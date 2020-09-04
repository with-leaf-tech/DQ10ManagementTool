﻿using Google.Apis.Services;
using Google.Apis.Vision.v1;
using Google.Apis.Vision.v1.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCRClassLibrary.OCR {
    public class GoogleVisionApiOCR : OcrBase {

        private VisionService visionService = null;

        public GoogleVisionApiOCR() {

        }

        public override void initialize(string accessKey, string baseLang) {
            base.initialize(accessKey, baseLang);
            Google.Apis.Auth.OAuth2.GoogleCredential credential = Google.Apis.Auth.OAuth2.GoogleCredential.FromFile(accessKey);
            credential = credential.CreateScoped(new[] { VisionService.Scope.CloudPlatform });

            visionService = new VisionService(new BaseClientService.Initializer {
                HttpClientInitializer = credential,
                GZipEnabled = false
            });
        }

        public override string GetTextFromImage(System.Drawing.Bitmap bitmap) {
            string returnText = "";
            //DetectTextWord(visionService, ImageToByte(bitmap), ref returnText);

            int result = 1;
            Console.WriteLine("Detecting image to texts...");
            // Convert image to Base64 encoded for JSON ASCII text based request
            string imageContent = Convert.ToBase64String(ImageToByte(bitmap));

            try {
                // Post text detection request to the Vision API
                var responses = visionService.Images.Annotate(
                    new BatchAnnotateImagesRequest() {
                        Requests = new[]
                        {
                          new AnnotateImageRequest()
                          {
                            Features = new []
                            { new Feature()
                              {
                                //Type = "TEXT_DETECTION"
                                Type = option.Length == 0 ? "TEXT_DETECTION" : "DOCUMENT_TEXT_DETECTION"
                              }
                            },
                            Image = new Google.Apis.Vision.v1.Data.Image()
                            {
                              Content = imageContent
                            }
                          }
                        }
                    }).Execute();

                if (responses.Responses != null) {
                    returnText = responses.Responses[0].TextAnnotations[0].Description;

                    Console.WriteLine("SUCCESS：Cloud Vision API Access.");
                    result = 0;
                }
                else {
                    returnText = "";
                    Console.WriteLine("ERROR : No text found.");
                    result = -1;
                }
            }
            catch {
                returnText = "";
                Console.WriteLine("ERROR : Not Access Cloud Vision API.");
                result = -1;
            }

            return returnText;
        }
    }
}