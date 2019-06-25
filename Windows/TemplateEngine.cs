using System;
using System.IO;
using System.Text.RegularExpressions;

public class TemplateEngine
{
    public TemplateEngine()
    { }
    public string Build_Custom_Template(string contentSource, string contentName, string contentType, string layout)
    {
        string generatedcontentPath = "";

        try
        {
            string appdataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Shadowmask\\";
            File.Copy(contentSource, appdataPath + "Wallpaper\\" + contentName, true);

            string htmlTemplate = File.ReadAllText(appdataPath + "Templates\\" + contentType + "_" + layout + ".html");

            string generatedContent = "";

            if (contentType.Contains("Image"))
            {
                Regex urlParser = new Regex(@"url\(([^)]+)\)");
                generatedContent = urlParser.Replace(htmlTemplate, "url(\"" + contentName + "\")");
            }

            if (contentType.Contains("Video"))
            {

                Regex urlParser = new Regex("src=" + "\"([^\"]*)\"");
                generatedContent = urlParser.Replace(htmlTemplate, "src=\"" + contentName + "\"");
            }
            
            generatedcontentPath = appdataPath + "Wallpaper\\" + contentName + ".html";

            File.WriteAllText(generatedcontentPath, generatedContent);
        }
        catch (IOException)
        {

        }
        
        return generatedcontentPath;
    }
}
