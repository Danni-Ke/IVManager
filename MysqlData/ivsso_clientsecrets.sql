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
-- Table structure for table `clientsecrets`
--

DROP TABLE IF EXISTS `clientsecrets`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `clientsecrets` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Description` varchar(2000) DEFAULT NULL,
  `Value` varchar(4000) NOT NULL,
  `Expiration` datetime DEFAULT NULL,
  `Type` varchar(250) NOT NULL,
  `Created` datetime NOT NULL,
  `ClientId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_ClientSecrets_ClientId` (`ClientId`),
  CONSTRAINT `FK_ClientSecrets_Clients_ClientId` FOREIGN KEY (`ClientId`) REFERENCES `clients` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `clientsecrets`
--

LOCK TABLES `clientsecrets` WRITE;
/*!40000 ALTER TABLE `clientsecrets` DISABLE KEYS */;
INSERT INTO `clientsecrets` VALUES (1,NULL,'yXoX2fDcczveivUB5mXw6ILsiIr0O4Gm/68oPemKtq4=',NULL,'SharedSecret','2019-06-17 11:40:12',2),(2,NULL,'a4ayc/80/OGda4BO/1o/V0etpOqiLx1JwB5S3beHW0s=',NULL,'SharedSecret','2019-06-17 11:40:13',3),(3,NULL,'K7gNU3sdo+OL0wNhqoVWhr3g6s1xYv72ol/pe/Unols=',NULL,'SharedSecret','2019-06-17 11:40:14',4),(4,NULL,'7/8UIDhKmWpRLeHTatK/vgVmKx6sS9XG5Ee5Uy1LujA=',NULL,'SharedSecret','2019-06-17 11:40:11',1);
/*!40000 ALTER TABLE `clientsecrets` ENABLE KEYS */;
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
