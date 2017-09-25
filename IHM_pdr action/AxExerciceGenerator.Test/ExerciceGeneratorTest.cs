//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using NUnit.Framework;
//using AxExerciceGenerator;
//using AxModel;
//using AxConfiguration;
//using AxModelExercice;
//namespace AxExerciceGenerator.Test
//{
//    [TestFixture]
//    class ExerciceGeneratorTest
//    {
//        string pathConfigSource,pathConfigDestination, pathBorneSource,pathBorneDestination;
//        ThemeEvaluationModel themeEnfant,themeDefaut;
//        ThemeModel themeJeu;
//        //Test sur la génération d'exercices
//        [SetUp]
//        public void Init()
//        {
//            pathConfigSource = "../../../AxView/Config/Base/";
//            pathConfigDestination = "../../Config/Base/";
//            pathBorneSource = "../../../AxView/Config/Borne/";
//            pathBorneDestination = "../../Config/Borne/";
//            DeploymentItem.DeployConfigs(pathConfigSource, pathConfigDestination);
//            DeploymentItem.DeployConfigs(pathBorneSource, pathBorneDestination);
//            themeEnfant = new ThemeEvaluationModel("enfant","cible","chasseur","fond");
//            themeDefaut = new ThemeEvaluationModel("defaut", "cible", "chasseur", "fond");
//            themeJeu = new ThemeModel();
//        }

//        #region Exercice Evaluation
//        //Cercle
//        [Test]
//        public void Exercice_Doit_Etre_Type_Cercle_Sans_Theme()
//        {
//            //ExerciceEvaluation exoEval = ExerciceGenerator.GetExerciceEvaluation("cercle", themeDefaut);
//            Circle ci = (Circle)exoEval;
//            Assert.AreEqual(ci.ThemeEnfant.Name, themeDefaut.Name);
//            Assert.AreEqual(ci.TypeForme, FormeType.Cercle);
//        }
//        [Test]
//        public void Exercice_Doit_Etre_Type_Cercle_Avec_Theme()
//        {
//            //ExerciceEvaluation exoEval = ExerciceGenerator.GetExerciceEvaluation("cercle", themeEnfant);
//            Circle ci = (Circle)exoEval;
//            Assert.AreEqual(ci.TypeForme, FormeType.Cercle);
//            Assert.AreEqual(ci.ThemeEnfant.Name, themeEnfant.Name);
//        }
//        //Carré
//        [Test]
//        public void Exercice_Doit_Etre_Type_Carre_Sans_Theme()
//        {
//            //ExerciceEvaluation exoEval = ExerciceGenerator.GetExerciceEvaluation("carre", themeDefaut);
//            Square sq = (Square)exoEval;
//            Assert.AreEqual(sq.TypeForme, FormeType.Carré);
//            Assert.AreEqual(sq.ThemeEnfant.Name, themeDefaut.Name);
//        }
//        [Test]
//        public void Exercice_Doit_Etre_Type_Carre_Avec_Theme()
//        {
//            //ExerciceEvaluation exoEval = ExerciceGenerator.GetExerciceEvaluation("carre", themeEnfant);
//            Square sq = (Square)exoEval;
//            Assert.AreEqual(sq.TypeForme, FormeType.Carré);
//            Assert.AreEqual(sq.ThemeEnfant.Name, themeEnfant.Name);
//        }
//        //Target
//        [Test]
//        public void Exercice_Doit_Etre_Type_Target_Sans_Theme()
//        {
//            //ExerciceEvaluation exoEval = ExerciceGenerator.GetExerciceEvaluation("target", themeDefaut);
//            Target tg = (Target)exoEval;
//            Assert.AreEqual(tg.TypeDroite, DroiteType.Vertical);
//            Assert.AreEqual(tg.ThemeEnfant.Name, themeDefaut.Name);
//        }
//        [Test]
//        public void Exercice_Doit_Etre_Type_Target_Avec_Theme()
//        {
//            //ExerciceEvaluation exoEval = ExerciceGenerator.GetExerciceEvaluation("target", themeEnfant);
//            Target tg = (Target)exoEval;
//            Assert.AreEqual(tg.TypeDroite, DroiteType.Vertical);
//            Assert.AreEqual(tg.ThemeEnfant.Name, themeEnfant.Name);
//        }
//        //FreeAmplitude
//        [Test]
//        public void Exercice_Doit_Etre_Type_FreeAmpl_Sans_Theme()
//        {
//            //ExerciceEvaluation exoEval = ExerciceGenerator.GetExerciceEvaluation("freeAmpl", themeDefaut);
//            FreeAmplitude fa = (FreeAmplitude)exoEval;
//            Assert.AreEqual(fa.TypeDroite, DroiteType.VerticalLong);
//            Assert.AreEqual(fa.ThemeEnfant.Name, themeDefaut.Name);
//        }
//        [Test]
//        public void Exercice_Doit_Etre_Type_FreeAmpl_Avec_Theme()
//        {
//            //ExerciceEvaluation exoEval = ExerciceGenerator.GetExerciceEvaluation("freeAmpl", themeEnfant);
//            FreeAmplitude tg = (FreeAmplitude)exoEval;
//            Assert.AreEqual(tg.TypeDroite, DroiteType.VerticalLong);
//            Assert.AreEqual(tg.ThemeEnfant.Name, themeEnfant.Name);
//        }
//        #endregion

//        #region Exercice Reeducation
//        //Jeu
//        [Test]
//        public void Jeu_Doit_Etre_Type_Jeu_Avec_Bon_Theme_Facile()
//        {
//            ExerciceJeu exoEval = ExerciceGenerator.GetExerciceJeu(themeJeu, "Facile");
//            Assert.AreEqual(exoEval.TypeExercice, ExerciceTypes.Jeu);
//            Assert.AreEqual(exoEval.Theme.Name, "UnnamedTheme");
//            Assert.AreEqual(exoEval.Niveau, 1);
//        }
//        [Test]
//        public void Jeu_Doit_Etre_Type_Jeu_Avec_Bon_Theme_Moyen()
//        {
//            ExerciceJeu exoEval = ExerciceGenerator.GetExerciceJeu(themeJeu, "Moyen");
//            Assert.AreEqual(exoEval.TypeExercice, ExerciceTypes.Jeu);
//            Assert.AreEqual(exoEval.Theme.Name, "UnnamedTheme");
//            Assert.AreEqual(exoEval.Niveau, 2);
//        }
//        [Test]
//        public void Jeu_Doit_Etre_Type_Jeu_Avec_Bon_Theme_Difficile()
//        {
//            ExerciceJeu exoEval = ExerciceGenerator.GetExerciceJeu(themeJeu, "Difficile");
//            Assert.AreEqual(exoEval.TypeExercice, ExerciceTypes.Jeu);
//            Assert.AreEqual(exoEval.Theme.Name, "UnnamedTheme");
//            Assert.AreEqual(exoEval.Niveau, 3);
//        }
//        [Test]
//        public void Jeu_Doit_Etre_Type_Jeu_Avec_Bon_Theme_Expert()
//        {
//            ExerciceJeu exoEval = ExerciceGenerator.GetExerciceJeu(themeJeu, "Expert");
//            Assert.AreEqual(exoEval.TypeExercice, ExerciceTypes.Jeu);
//            Assert.AreEqual(exoEval.Theme.Name, "UnnamedTheme");
//            Assert.AreEqual(exoEval.Niveau, 4);
//        }
//        #endregion

//        #region Configuration
//        ////Small Borne
//        [Test]
//        public void Exercice_Eval_Doit_Avoir_Big_Borne()
//        {
//            //ExerciceEvaluation exoEval = ExerciceGenerator.GetExerciceEvaluation("target", themeEnfant);
//            ExerciceBorneConfig config = ExerciceConfig.GetBigBorne();
//            Assert.AreEqual(exoEval.BorneConfig.BorneArc_B, config.BorneArc_B);
//            Assert.AreEqual(exoEval.BorneConfig.BorneArc_H, config.BorneArc_H);
//            Assert.AreEqual(exoEval.BorneConfig.BorneB, config.BorneB);
//            Assert.AreEqual(exoEval.BorneConfig.BorneD, config.BorneD);
//            Assert.AreEqual(exoEval.BorneConfig.BorneG, config.BorneG);
//            Assert.AreEqual(exoEval.BorneConfig.BorneH, config.BorneH);
//        }
//        ////Base Config
//        [Test]
//        public void Exercice_Doit_Avoir_Base_Config()
//        {
//            //ExerciceEvaluation exoEval = ExerciceGenerator.GetExerciceEvaluation("target", themeEnfant);
//            ExerciceBaseConfig config = ExerciceConfig.GetBaseConfig();
//            Assert.AreEqual(exoEval.BaseConfig.Auto, config.Auto);
//            Assert.AreEqual(exoEval.BaseConfig.Init, config.Init);
//            Assert.AreEqual(exoEval.BaseConfig.Masse, config.Masse);
//            Assert.AreEqual(exoEval.BaseConfig.ModeType, config.ModeType);
//            Assert.AreEqual(exoEval.BaseConfig.NbrRep, config.NbrRep);
//            Assert.AreEqual(exoEval.BaseConfig.RaideurLat, config.RaideurLat);
//            Assert.AreEqual(exoEval.BaseConfig.RaideurLong, config.RaideurLong);
//            Assert.AreEqual(exoEval.BaseConfig.Viscosite, config.Viscosite);
//            Assert.AreEqual(exoEval.BaseConfig.Vitesse, config.Vitesse);
//        }
//        #endregion
//    }
//}
