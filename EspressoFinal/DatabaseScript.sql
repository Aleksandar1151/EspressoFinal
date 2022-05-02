CREATE DATABASE  IF NOT EXISTS `espressodb` /*!40100 DEFAULT CHARACTER SET utf8 */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `espressodb`;
-- MySQL dump 10.13  Distrib 8.0.27, for Win64 (x86_64)
--
-- Host: localhost    Database: espressodb
-- ------------------------------------------------------
-- Server version	8.0.27

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `artikal`
--

DROP TABLE IF EXISTS `artikal`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `artikal` (
  `idArtikal` int NOT NULL AUTO_INCREMENT,
  `naziv` varchar(45) DEFAULT NULL,
  `cijena` varchar(45) DEFAULT NULL,
  `kolicina` varchar(45) DEFAULT NULL,
  `Kategorija_idKategorija` int NOT NULL,
  PRIMARY KEY (`idArtikal`,`Kategorija_idKategorija`),
  KEY `fk_Artikal_Kategorija1_idx` (`Kategorija_idKategorija`),
  CONSTRAINT `fk_Artikal_Kategorija1` FOREIGN KEY (`Kategorija_idKategorija`) REFERENCES `kategorija` (`idKategorija`)
) ENGINE=InnoDB AUTO_INCREMENT=27 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `artikal`
--

LOCK TABLES `artikal` WRITE;
/*!40000 ALTER TABLE `artikal` DISABLE KEYS */;
INSERT INTO `artikal` VALUES (1,'Espresso','1.50','39',1),(2,'Čaj','2','40',1),(3,'Topla čokolada','3','20',1),(4,'Jagoda','1.8','3',2),(5,'Nes','2','8',1),(6,'Jabuka','2.5','12',2),(7,'Ananas','3','4',2),(8,'Cappuccino','2','200',1),(9,'Domaća kafa','2.50','299',1);
/*!40000 ALTER TABLE `artikal` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `kategorija`
--

DROP TABLE IF EXISTS `kategorija`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `kategorija` (
  `idKategorija` int NOT NULL AUTO_INCREMENT,
  `naziv` varchar(45) DEFAULT NULL,
  `boja` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`idKategorija`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `kategorija`
--

LOCK TABLES `kategorija` WRITE;
/*!40000 ALTER TABLE `kategorija` DISABLE KEYS */;
INSERT INTO `kategorija` VALUES (1,'Topli napitci',NULL),(2,'Bezalkoholna pića',NULL);
/*!40000 ALTER TABLE `kategorija` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `nalog`
--

DROP TABLE IF EXISTS `nalog`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `nalog` (
  `idNalog` int NOT NULL AUTO_INCREMENT,
  `ime` varchar(45) DEFAULT NULL,
  `lozinka` varchar(45) DEFAULT NULL,
  `privilegije` tinyint DEFAULT NULL,
  PRIMARY KEY (`idNalog`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `nalog`
--

LOCK TABLES `nalog` WRITE;
/*!40000 ALTER TABLE `nalog` DISABLE KEYS */;
INSERT INTO `nalog` VALUES (1,'admin','admin',1),(3,'test','test',0);
/*!40000 ALTER TABLE `nalog` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `otpis`
--

DROP TABLE IF EXISTS `otpis`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `otpis` (
  `idOtpis` int NOT NULL AUTO_INCREMENT,
  `kolicina` varchar(45) DEFAULT NULL,
  `datum` varchar(45) DEFAULT NULL,
  `Nalog_idNalog` int NOT NULL,
  `Artikal_idArtikal` int NOT NULL,
  PRIMARY KEY (`idOtpis`),
  KEY `fk_Otpis_Nalog1_idx` (`Nalog_idNalog`),
  KEY `fk_Otpis_Artikal1_idx` (`Artikal_idArtikal`),
  CONSTRAINT `fk_Otpis_Artikal1` FOREIGN KEY (`Artikal_idArtikal`) REFERENCES `artikal` (`idArtikal`),
  CONSTRAINT `fk_Otpis_Nalog1` FOREIGN KEY (`Nalog_idNalog`) REFERENCES `nalog` (`idNalog`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `otpis`
--

LOCK TABLES `otpis` WRITE;
/*!40000 ALTER TABLE `otpis` DISABLE KEYS */;
INSERT INTO `otpis` VALUES (1,'2','20-04-2022',1,6),(2,'3','24-04-2022',1,6),(3,'10','26-04-2022',1,4),(4,'2','01-05-2022',1,2);
/*!40000 ALTER TABLE `otpis` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `racun`
--

DROP TABLE IF EXISTS `racun`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `racun` (
  `idRacun` int NOT NULL AUTO_INCREMENT,
  `datum` varchar(45) DEFAULT NULL,
  `Nalog_idNalog` int NOT NULL,
  PRIMARY KEY (`idRacun`,`Nalog_idNalog`),
  KEY `fk_Racun_Nalog1_idx` (`Nalog_idNalog`),
  CONSTRAINT `fk_Racun_Nalog1` FOREIGN KEY (`Nalog_idNalog`) REFERENCES `nalog` (`idNalog`)
) ENGINE=InnoDB AUTO_INCREMENT=56 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `racun`
--

LOCK TABLES `racun` WRITE;
/*!40000 ALTER TABLE `racun` DISABLE KEYS */;
INSERT INTO `racun` VALUES (2,'17-01-2022',1),(3,'17-01-2022',1),(4,'17-01-2022',1),(5,'17-01-2022',1),(6,'17-01-2022',1),(7,'17-01-2022',1),(8,'17-01-2022',1),(9,'17-01-2022',1),(10,'17-01-2022',1),(11,'17-01-2022',1),(12,'17-01-2022',1),(13,'17-01-2022',1),(14,'17-01-2022',1),(15,'17-01-2022',1),(16,'17-01-2022',1),(17,'17-01-2022',1),(18,'17-01-2022',1),(19,'18-01-2022',1),(20,'30-04-2022',1),(21,'30-04-2022',1),(22,'30-04-2022',1),(23,'30-04-2022',1),(24,'30-04-2022',1),(25,'30-04-2022',1),(26,'30-04-2022',1),(27,'30-04-2022',1),(28,'30-04-2022',1),(29,'30-04-2022',1),(30,'30-04-2022',1),(31,'30-04-2022',1),(32,'30-04-2022',1),(33,'30-04-2022',1),(34,'30-04-2022',1),(35,'30-04-2022',1),(36,'30-04-2022',1),(37,'30-04-2022',1),(38,'30-04-2022',1),(39,'30-04-2022',1),(40,'30-04-2022',1),(41,'30-04-2022',1),(42,'30-04-2022',1),(43,'30-04-2022',1),(44,'30-04-2022',1),(45,'30-04-2022',1),(46,'30-04-2022',1),(47,'30-04-2022',1),(48,'01-05-2022',1),(49,'01-05-2022',1),(50,'01-05-2022',1),(51,'30-04-2022',1),(52,'30-04-2022',1),(53,'30-04-2022',1),(54,'02-05-2022',3),(55,'02-05-2022',3);
/*!40000 ALTER TABLE `racun` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `stavka`
--

DROP TABLE IF EXISTS `stavka`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `stavka` (
  `Racun_idRacun` int NOT NULL,
  `Artikal_idArtikal` int NOT NULL,
  `naziv` varchar(45) DEFAULT NULL,
  `kolicina` varchar(45) DEFAULT NULL,
  `cijena` varchar(45) DEFAULT NULL,
  `StorniranRacun_idStorniranRacun` int DEFAULT NULL,
  KEY `fk_Racun_has_Artikal_Artikal1_idx` (`Artikal_idArtikal`),
  KEY `fk_Racun_has_Artikal_Racun_idx` (`Racun_idRacun`),
  KEY `fk_Stavka_StorniranRacun1_idx` (`StorniranRacun_idStorniranRacun`),
  CONSTRAINT `fk_Racun_has_Artikal_Artikal1` FOREIGN KEY (`Artikal_idArtikal`) REFERENCES `artikal` (`idArtikal`),
  CONSTRAINT `fk_Racun_has_Artikal_Racun` FOREIGN KEY (`Racun_idRacun`) REFERENCES `racun` (`idRacun`),
  CONSTRAINT `fk_Stavka_StorniranRacun1` FOREIGN KEY (`StorniranRacun_idStorniranRacun`) REFERENCES `storniranracun` (`idStorniranRacun`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `stavka`
--

LOCK TABLES `stavka` WRITE;
/*!40000 ALTER TABLE `stavka` DISABLE KEYS */;
INSERT INTO `stavka` VALUES (3,1,'Espresso','2','1.5',NULL),(3,2,'Čaj','1','2',NULL),(3,1,'Espresso','2','1.5',2),(3,1,'Espresso','1','1.5',3),(4,2,'Čaj','1','2',4),(4,1,'Espresso','1','1.5',4),(5,5,'Nes','1','2',NULL),(5,1,'Espresso','1','1.5',NULL),(6,3,'Topla čokolada','2','3',NULL),(7,3,'Topla čokolada','2','3',NULL),(7,2,'Čaj','1','2',NULL),(8,5,'Nes','1','2',NULL),(8,2,'Čaj','1','2',NULL),(8,1,'Espresso','1','1.5',NULL),(9,2,'Čaj','2','2',NULL),(10,4,'Jagoda','1','1.8',NULL),(10,1,'Espresso','1','1.5',NULL),(10,5,'Nes','1','2',NULL),(11,4,'Jagoda','1','1.8',5),(12,1,'Espresso','1','1.5',6),(13,2,'Čaj','1','2',7),(14,4,'Jagoda','2','1.8',NULL),(15,4,'Jagoda','2','1.8',8),(16,4,'Jagoda','2','1.8',9),(17,6,'Jabuku','2','2.5',NULL),(18,6,'Jabuka','2','2.5',NULL),(19,6,'Jabuka','2','2.5',NULL),(19,4,'Jagoda','1','1.8',NULL),(20,1,'Espresso','2','1.5',NULL),(21,1,'Espresso','1','1.5',NULL),(22,1,'Espresso','1','1.5',NULL),(23,1,'Espresso','1','1.5',NULL),(24,1,'Espresso','1','1.5',NULL),(25,2,'Čaj','1','2',NULL),(26,1,'Espresso','1','1.5',NULL),(26,2,'Čaj','1','2',NULL),(27,1,'Espresso','1','1.5',NULL),(27,2,'Čaj','1','2',NULL),(28,1,'Espresso','1','1.5',NULL),(28,5,'Nes','1','2',NULL),(28,2,'Čaj','1','2',NULL),(28,3,'Topla čokolada','1','3',NULL),(29,1,'Espresso','1','1.5',NULL),(29,5,'Nes','1','2',NULL),(29,2,'Čaj','1','2',NULL),(30,3,'Topla čokolada','1','3',NULL),(30,2,'Čaj','1','2',NULL),(30,1,'Espresso','1','1.5',NULL),(30,5,'Nes','1','2',NULL),(31,2,'Čaj','1','2',NULL),(32,3,'Topla čokolada','1','3',NULL),(32,1,'Espresso','1','1.5',NULL),(33,3,'Topla čokolada','1','3',NULL),(33,5,'Nes','1','2',NULL),(33,1,'Espresso','1','1.5',NULL),(33,2,'Čaj','1','2',NULL),(34,1,'Espresso','1','1.5',NULL),(34,4,'Jagoda','1','1.8',NULL),(34,6,'Jabuka','1','2.5',NULL),(34,3,'Topla čokolada','1','3',NULL),(35,1,'Espresso','1','1.5',NULL),(35,2,'Čaj','1','2',NULL),(36,1,'Espresso','1','1.5',NULL),(36,2,'Čaj','1','2',NULL),(37,1,'Espresso','1','1.5',NULL),(37,2,'Čaj','1','2',NULL),(38,1,'Espresso','1','1.5',NULL),(38,2,'Čaj','1','2',NULL),(39,1,'Espresso','1','1.5',NULL),(39,2,'Čaj','1','2',NULL),(40,1,'Espresso','1','1.5',NULL),(40,2,'Čaj','1','2',NULL),(41,1,'Espresso','1','1.5',NULL),(41,2,'Čaj','1','2',NULL),(42,1,'Espresso','1','1.5',NULL),(42,2,'Čaj','1','2',NULL),(43,1,'Espresso','1','1.5',NULL),(43,2,'Čaj','1','2',NULL),(44,1,'Espresso','1','1.5',NULL),(44,2,'Čaj','1','2',NULL),(45,1,'Espresso','1','1.5',NULL),(45,2,'Čaj','1','2',NULL),(45,4,'Jagoda','1','1.8',NULL),(46,1,'Espresso','1','1.5',NULL),(46,2,'Čaj','1','2',NULL),(47,1,'Espresso','1','1.5',NULL),(47,2,'Čaj','1','2',NULL),(47,3,'Topla čokolada','1','3',NULL),(48,1,'Espresso','1','1.5',NULL),(48,2,'Čaj','1','2',NULL),(48,3,'Topla čokolada','1','3',NULL),(49,2,'Čaj','1','2',10),(50,1,'Espresso','1','1.5',NULL),(50,2,'Čaj','2','2',NULL),(51,2,'Čaj','1','2',11),(52,3,'Topla čokolada','1','3',12),(53,2,'Čaj','1','2',13),(54,2,'Čaj','1','2',NULL),(54,9,'Domaća kafa','2','2.5',NULL),(55,9,'Domaća kafa','1','2.5',14);
/*!40000 ALTER TABLE `stavka` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `storniranracun`
--

DROP TABLE IF EXISTS `storniranracun`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `storniranracun` (
  `idStorniranRacun` int NOT NULL AUTO_INCREMENT,
  `Racun_idRacun` int NOT NULL,
  PRIMARY KEY (`idStorniranRacun`),
  KEY `fk_StorniranRacun_Racun1_idx` (`Racun_idRacun`),
  CONSTRAINT `fk_StorniranRacun_Racun1` FOREIGN KEY (`Racun_idRacun`) REFERENCES `racun` (`idRacun`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `storniranracun`
--

LOCK TABLES `storniranracun` WRITE;
/*!40000 ALTER TABLE `storniranracun` DISABLE KEYS */;
INSERT INTO `storniranracun` VALUES (1,3),(2,3),(3,3),(4,4),(5,11),(6,12),(7,13),(8,15),(9,16),(10,49),(11,51),(12,52),(13,53),(14,55);
/*!40000 ALTER TABLE `storniranracun` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2022-05-02 10:06:25
