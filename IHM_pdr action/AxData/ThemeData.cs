using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AxModel;
using System.Xml.Linq;
using System.IO;
using System.Collections.ObjectModel;


namespace AxData
{
    public static class ThemeData
    {
        public static List<ThemeModel> LoadAllReeducationTheme()
        {
            XDocument fileTheme = XDocument.Load(@"../../Theme/listeThemesReeducation.xml");
            var th = from theme in fileTheme.Descendants("Theme") orderby theme.Attribute("Nom") select theme;
            List<ThemeModel> listeTheme = new List<ThemeModel>();
            foreach (var theme in th)
            {
                string nom = (string)theme.Element("Nom");
                string cheminCible = "../../Theme/Reeducation/" + nom + "/Cible/cible.png";
                string cheminCibleDynamic = "../../Theme/Reeducation/" + nom + "/Cible/cibleDynamic.png";
                string nomCible = (string)theme.Element("NomCible");
                string cheminChasseur = "../../Theme/Reeducation/" + nom + "/Chasseur/shape.png";
                string cheminChasseurHit = "../../Theme/Reeducation/" + nom + "/Chasseur/shapeHit.png";
                string nomChasseur = (string)theme.Element("NomChasseur");
                string sonChasseur = "../../Theme/Reeducation/" + nom + "/son/sound.wav";
                string fond = "../../Theme/Reeducation/" + nom + "/Fond/background.jpg";
                string fondDynamic = "../../Theme/Reeducation/" + nom + "/Fond/backgroundDynamic.jpg";
                string genre = (string)theme.Element("Genre");
                CibleModel cible = new CibleModel(cheminCible, cheminCible, nomCible);
                CibleModel cibleDynamic = new CibleModel(cheminCibleDynamic, cheminCibleDynamic, nomCible);
                ChasseurModel chasseur = new ChasseurModel(cheminChasseur, cheminChasseurHit, nomChasseur, sonChasseur);
                ThemeModel themeMod = new ThemeModel(nom,cible,cibleDynamic,chasseur,fond,fondDynamic,genre);
                listeTheme.Add(themeMod);
            }
            return listeTheme;
        }

        public static List<ThemeEvaluationModel> LoadAllEvaluationTheme(string nomFond)
        {
            XDocument fileTheme = XDocument.Load(@"../../Theme/listeThemesEvaluation.xml");
            var th = from theme in fileTheme.Descendants("Theme") select theme;
            List<ThemeEvaluationModel> listeTheme = new List<ThemeEvaluationModel>();
            foreach (var theme in th)
            {
                string nom = (string)theme.Element("Nom");
                if (nomFond == "ParDefaut.jpg")
                    nom = "Defaut";
                string cheminCible = "../../Theme/Evaluation/" + nom + "/Cible/cible.png";
                string cheminChasseur = "../../Theme/Evaluation/" + nom + "/Chasseur/shape.png";
                string fond = "../../Theme/Evaluation/Fond/"+ nomFond;
                nom = (string)theme.Element("Nom");
                if (nomFond == "ParDefaut.jpg" && nom == "Cercle")
                    cheminCible = "";
                ThemeEvaluationModel themeEval = new ThemeEvaluationModel(nom,cheminCible,cheminChasseur,fond);
                if (nom == "Cible")
                    themeEval.Chasseur2 = "../../Theme/Evaluation/" + nom + "/Chasseur/shape2.png";
                
                listeTheme.Add(themeEval);
            }
            return listeTheme;
        }

        public static ObservableCollection<string> LoadAllFondEvaluation()
        {
            ObservableCollection<string> listeFond = new ObservableCollection<string>();
            foreach (string file in Directory.GetFiles("../../Theme/Evaluation/Fond/"))
            {
                string nom = "../../Theme/Evaluation/Fond/"+Path.GetFileName(file);
                if(Path.GetFileName(file) != "background_blank.png")
                    listeFond.Add(nom);
            }
            return listeFond;
        }

        public static ObservableCollection<string> LoadDefaultFondEvaluation()
        {
            ObservableCollection<string> listeFond = new ObservableCollection<string>();
            foreach (string file in Directory.GetFiles("../../Theme/Evaluation/Fond/"))
            {
                string nom = "../../Theme/Evaluation/Fond/" + Path.GetFileName(file);
                if (Path.GetFileName(file) == "ParDefaut.jpg")
                    listeFond.Add(nom);
            }
            return listeFond;
        }
    }
}
