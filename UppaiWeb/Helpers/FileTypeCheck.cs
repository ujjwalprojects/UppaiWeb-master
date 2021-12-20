using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace UppaiWeb.Helpers
{
    public static class FileTypeCheck
    {
        public static string IsValidFile(this HttpPostedFileBase postedFile, string fileType)
        {
            if (postedFile == null)
            {
                return "Select a file";
            }
            switch (fileType)
            {
                case "Image":
                    {
                        //-------------------------------------------
                        //  Check the image mime types
                        //-------------------------------------------
                        if (postedFile.ContentType.ToLower() != "image/jpg" &&
                                    postedFile.ContentType.ToLower() != "image/jpeg" &&
                                    postedFile.ContentType.ToLower() != "image/pjpeg" &&
                                    postedFile.ContentType.ToLower() != "image/x-png" &&
                                    postedFile.ContentType.ToLower() != "image/png")
                        {
                            return "Please select an image file";
                        }

                        //-------------------------------------------
                        //  Check the image extension
                        //-------------------------------------------
                        if (Path.GetExtension(postedFile.FileName).ToLower() != ".jpg"
                            && Path.GetExtension(postedFile.FileName).ToLower() != ".png"
                            && Path.GetExtension(postedFile.FileName).ToLower() != ".jpeg")
                        {
                            return "Please select an image file";
                        }
                        try
                        {
                            if (!postedFile.InputStream.CanRead)
                            {
                                return "Please select an image file";
                            }

                            if (postedFile.ContentLength > 2097152)
                            {
                                return "Please select an image with maximum size of 2MB.";
                            }

                            byte[] buffer = new byte[512];
                            postedFile.InputStream.Read(buffer, 0, 512);
                            string content = System.Text.Encoding.UTF8.GetString(buffer);
                            if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                                RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
                            {
                                return "Please select an image file";
                            }
                        }
                        catch (Exception)
                        {
                            return "Please select an image file";
                        }

                        try
                        {
                            using (var bitmap = new System.Drawing.Bitmap(postedFile.InputStream))
                            {
                            }
                        }
                        catch (Exception)
                        {
                            return "Please select an image file";
                        }
                        finally
                        {
                            postedFile.InputStream.Position = 0;
                        }

                        return "Success";
                    }
                case "Video":
                    {
                        //-------------------------------------------
                        //  Check the video mime types
                        //-------------------------------------------
                        if (postedFile.ContentType.ToLower() != "video/mp4")
                        {
                            return "Please select a video file";
                        }

                        //-------------------------------------------
                        //  Check the video extension
                        //-------------------------------------------
                        if (Path.GetExtension(postedFile.FileName).ToLower() != ".mp4")
                        {
                            return "Please select a video file";
                        }
                        try
                        {
                            if (!postedFile.InputStream.CanRead)
                            {
                                return "Please select a video file";
                            }

                            if (postedFile.ContentLength > 5242880)
                            {
                                return "Please select a video with maximum size of 5MB.";
                            }

                            byte[] buffer = new byte[512];
                            postedFile.InputStream.Read(buffer, 0, 512);
                            string content = System.Text.Encoding.UTF8.GetString(buffer);
                            if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                                RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
                            {
                                return "Please select a video file";
                            }
                        }
                        catch (Exception)
                        {
                            return "Please select a video file";
                        }

                        return "Success";
                    }
                case "File":
                    {
                        //-------------------------------------------
                        //  Check the file mime types
                        //-------------------------------------------
                        if (postedFile.ContentType.ToLower() != "application/pdf")
                        {
                            return "Please select a valid file";
                        }

                        //-------------------------------------------
                        //  Check the file extension
                        //-------------------------------------------
                        if (Path.GetExtension(postedFile.FileName).ToLower() != ".pdf")
                        {
                            return "Please select a valid PDF file";
                        }
                        try
                        {
                            if (!postedFile.InputStream.CanRead)
                            {
                                return "Please select a valid PDF file";
                            }

                            if (postedFile.ContentLength > 41943040)
                            {
                                return "Please select a file with maximum size of 40MB.";
                            }

                            byte[] buffer = new byte[512];
                            postedFile.InputStream.Read(buffer, 0, 512);
                            string content = System.Text.Encoding.UTF8.GetString(buffer);
                            string file = content.Substring(0, 20);
                            if (!file.Contains("PDF"))
                            {
                                return "Please select a valid PDF file";
                            }

                        }
                        catch (Exception)
                        {
                            return "Please select a valid PDF file";
                        }

                        return "Success";
                    }
                default:
                    return "Invalid file";
            }

        }
        private static readonly Regex DataUriPattern = new Regex(@"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                                RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline);
        public static bool DataImage(string dataUri)
        {
            if (string.IsNullOrWhiteSpace(dataUri)) return false;
            try
            {
                Match match = DataUriPattern.Match(dataUri);
                if (match.Success)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}