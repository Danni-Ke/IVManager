-- MySQL dump 10.13  Distrib 8.0.16, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: ivsso
-- ------------------------------------------------------
-- Server version	5.7.24-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
 SET NAMES utf8 ;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `identityresources`
--

DROP TABLE IF EXISTS `identityresources`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `identityresources` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Enabled` bit(1) NOT NULL,
  `Name` varchar(200) NOT NULL,
  `DisplayName` varchar(200) DEFAULT NULL,
  `Description` varchar(1000) DEFAULT NULL,
  `Required` bit(1) NOT NULL,
  `Emphasize` bit(1) NOT NULL,
  `ShowInDiscoveryDocument` bit(1) NOT NULL,
  `Created` datetime NOT NULL,
  `Updated` datetime DEFAULT NULL,
  `NonEditable` bit(1) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_IdentityResources_Name` (`Name`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `identityresources`
--

LOCK TABLES `identityresources` WRITE;
/*!40000 ALTER TABLE `identityresources` DISABLE KEYS */;
INSERT INTO `identityresources` VALUES (1,_binary '','openid','Your user identifier',NULL,_binary '',_binary '\0',_binary '','2019-06-17 11:40:18',NULL,_binary '\0'),(2,_binary '','profile','User profile','Your user profile information (first name, last name, etc.)',_binary '\0',_binary '',_binary '','2019-06-17 11:40:19',NULL,_binary '\0');
/*!40000 ALTER TABLE `identityresources` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2019-06-28 11:56:25
