using Newtonsoft.Json;
using Vids.Model;

namespace Vids.Service
{
    public interface ITranslatorService
    {
        Lang Zh { get; set; }
        Lang En { get; set; }
        Lang Ms { get; set; }

        Lang Lang(string lang);
    }

    public class TranslatorService : ITranslatorService
    {
        public Lang En { get; set; } = new Lang();
        public Lang Zh { get; set; } = new Lang();  
        public Lang Ms { get; set; } = new Lang();  

        public TranslatorService(IConfiguration configuration)
        {
            // read lang.json file from i18n folder
            var contentRoot = configuration.GetValue<string>(WebHostDefaults.ContentRootKey);
            var i18nFolder = Path.Combine(contentRoot, "i18n");

            if(Directory.Exists(i18nFolder))
            {
                string enFilePath = Path.Combine(i18nFolder, "en.json");
                string msFilePath = Path.Combine(i18nFolder, "ms.json");
                string zhFilePath = Path.Combine(i18nFolder, "zh.json");

                En = LoadJson(enFilePath);
                Ms = LoadJson(msFilePath);
                Zh = LoadJson(zhFilePath);  
            }
        }

        public Lang Lang(string lang)
        {
            Lang currentLang = En;

            switch (lang)
            {
                case "en":
                default:
                    currentLang = En;
                    break;
                case "zh":
                    currentLang = Zh;
                    break;
                case "ms":
                    currentLang = Ms;
                    break;
            }

            return currentLang;
        }

        private Lang LoadJson(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    return new Lang();
                }

                using (StreamReader r = new StreamReader(filePath))
                {
                    string json = r.ReadToEnd();
                    Lang? lang = JsonConvert.DeserializeObject<Lang>(json);

                    if(lang != null)
                        return lang;
                    else 
                        return new Lang();
                }
            }
            catch (Exception ex)
            {
                var err = ex;
                return new Lang();
            }
          
        }
    }
}
