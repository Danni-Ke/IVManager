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
-- Table structure for table `clients`
--

DROP TABLE IF EXISTS `clients`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `clients` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Enabled` bit(1) NOT NULL,
  `ClientId` varchar(200) NOT NULL,
  `ProtocolType` varchar(200) NOT NULL,
  `RequireClientSecret` bit(1) NOT NULL,
  `ClientName` varchar(200) DEFAULT NULL,
  `Description` varchar(1000) DEFAULT NULL,
  `ClientUri` varchar(2000) DEFAULT NULL,
  `LogoUri` varchar(2000) DEFAULT NULL,
  `RequireConsent` bit(1) NOT NULL,
  `AllowRememberConsent` bit(1) NOT NULL,
  `AlwaysIncludeUserClaimsInIdToken` bit(1) NOT NULL,
  `RequirePkce` bit(1) NOT NULL,
  `AllowPlainTextPkce` bit(1) NOT NULL,
  `AllowAccessTokensViaBrowser` bit(1) NOT NULL,
  `FrontChannelLogoutUri` varchar(2000) DEFAULT NULL,
  `FrontChannelLogoutSessionRequired` bit(1) NOT NULL,
  `BackChannelLogoutUri` varchar(2000) DEFAULT NULL,
  `BackChannelLogoutSessionRequired` bit(1) NOT NULL,
  `AllowOfflineAccess` bit(1) NOT NULL,
  `IdentityTokenLifetime` int(11) NOT NULL,
  `AccessTokenLifetime` int(11) NOT NULL,
  `AuthorizationCodeLifetime` int(11) NOT NULL,
  `ConsentLifetime` int(11) DEFAULT NULL,
  `AbsoluteRefreshTokenLifetime` int(11) NOT NULL,
  `SlidingRefreshTokenLifetime` int(11) NOT NULL,
  `RefreshTokenUsage` int(11) NOT NULL,
  `UpdateAccessTokenClaimsOnRefresh` bit(1) NOT NULL,
  `RefreshTokenExpiration` int(11) NOT NULL,
  `AccessTokenType` int(11) NOT NULL,
  `EnableLocalLogin` bit(1) NOT NULL,
  `IncludeJwtId` bit(1) NOT NULL,
  `AlwaysSendClientClaims` bit(1) NOT NULL,
  `ClientClaimsPrefix` varchar(200) DEFAULT NULL,
  `PairWiseSubjectSalt` varchar(200) DEFAULT NULL,
  `Created` datetime NOT NULL,
  `Updated` datetime DEFAULT NULL,
  `LastAccessed` datetime DEFAULT NULL,
  `UserSsoLifetime` int(11) DEFAULT NULL,
  `UserCodeType` varchar(100) DEFAULT NULL,
  `DeviceCodeLifetime` int(11) NOT NULL,
  `NonEditable` bit(1) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_Clients_ClientId` (`ClientId`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `clients`
--

LOCK TABLES `clients` WRITE;
/*!40000 ALTER TABLE `clients` DISABLE KEYS */;
INSERT INTO `clients` VALUES (1,_binary '','Kong API','oidc',_binary '',NULL,NULL,NULL,NULL,_binary '',_binary '',_binary '\0',_binary '\0',_binary '\0',_binary '\0',NULL,_binary '',NULL,_binary '',_binary '\0',300,3600,300,NULL,2592000,1296000,1,_binary '\0',1,0,_binary '',_binary '\0',_binary '\0','client_',NULL,'2019-06-17 11:40:11',NULL,NULL,NULL,NULL,300,_binary '\0'),(2,_binary '','Baidu API','oidc',_binary '',NULL,NULL,NULL,NULL,_binary '',_binary '',_binary '\0',_binary '\0',_binary '\0',_binary '\0',NULL,_binary '',NULL,_binary '',_binary '\0',300,3600,300,NULL,2592000,1296000,1,_binary '\0',1,0,_binary '',_binary '\0',_binary '\0','client_',NULL,'2019-06-17 11:40:12',NULL,NULL,NULL,NULL,300,_binary '\0'),(3,_binary '','Meitu API','oidc',_binary '',NULL,NULL,NULL,NULL,_binary '',_binary '',_binary '\0',_binary '\0',_binary '\0',_binary '\0',NULL,_binary '',NULL,_binary '',_binary '\0',300,3600,300,NULL,2592000,1296000,1,_binary '\0',1,0,_binary '',_binary '\0',_binary '\0','client_',NULL,'2019-06-17 11:40:13',NULL,NULL,NULL,NULL,300,_binary '\0'),(4,_binary '','mvc','oidc',_binary '','MVC Client',NULL,NULL,NULL,_binary '\0',_binary '',_binary '\0',_binary '\0',_binary '\0',_binary '\0','http://localhost:5002/Home/Logout',_binary '','http://localhost:5002/Home/Logout',_binary '',_binary '\0',300,3600,300,NULL,2592000,1296000,1,_binary '\0',1,0,_binary '',_binary '\0',_binary '\0','client_',NULL,'2019-06-17 11:40:14',NULL,NULL,NULL,NULL,300,_binary '\0');
/*!40000 ALTER TABLE `clients` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2019-06-28 11:56:23
