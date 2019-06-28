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
-- Table structure for table `aspnetusers`
--

DROP TABLE IF EXISTS `aspnetusers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `aspnetusers` (
  `Id` varchar(255) NOT NULL,
  `UserName` varchar(256) DEFAULT NULL,
  `NormalizedUserName` varchar(256) DEFAULT NULL,
  `Email` varchar(256) DEFAULT NULL,
  `NormalizedEmail` varchar(256) DEFAULT NULL,
  `EmailConfirmed` bit(1) NOT NULL,
  `PasswordHash` longtext,
  `SecurityStamp` longtext,
  `ConcurrencyStamp` longtext,
  `PhoneNumber` longtext,
  `PhoneNumberConfirmed` bit(1) NOT NULL,
  `TwoFactorEnabled` bit(1) NOT NULL,
  `LockoutEnd` datetime(6) DEFAULT NULL,
  `LockoutEnabled` bit(1) NOT NULL,
  `AccessFailedCount` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UserNameIndex` (`NormalizedUserName`),
  KEY `EmailIndex` (`NormalizedEmail`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetusers`
--

LOCK TABLES `aspnetusers` WRITE;
/*!40000 ALTER TABLE `aspnetusers` DISABLE KEYS */;
INSERT INTO `aspnetusers` VALUES ('0d2b7958-d29b-4c8b-a8d4-927667654322','pineappleman520@gmail.com','PINEAPPLEMAN520@GMAIL.COM','pineappleman520@gmail.com','PINEAPPLEMAN520@GMAIL.COM',_binary '\0','Gnm19980521!','YSHUMSEC2D7OVD3L7A4PLIKOYHAQJKJB','091d33e7-312c-45e6-877a-b82e4f726d47',NULL,_binary '\0',_binary '\0',NULL,_binary '',0),('127aa045-7b7e-43cc-b567-bf18be3543b6','1111@qq.com','1111@QQ.COM','1111@qq.com','1111@QQ.COM',_binary '\0','Gnm19980521!','CF74QVTIZSIDZTESQOQTZNEFI7WFZAL5','2d48592d-8757-4fa9-91fc-4ffca62d52d0','18259480501',_binary '\0',_binary '\0','2019-05-21 15:20:30.000000',_binary '',0),('1f571072-f669-45fe-9537-be485d8fe0f9','985@qq.com','985@QQ.COM','985@qq.com','985@QQ.COM',_binary '\0','Gnm19980521!','OWSOTDCBRJLHT562NUY26JARVAGVYVWR','1656316f-a718-45f6-8058-dffe2ff1dada',NULL,_binary '\0',_binary '\0',NULL,_binary '',0),('9de849f5-87dd-4fdb-8d0d-025eb9cd086a','3333@qq.com','3333@QQ.COM','3333@qq.com','3333@QQ.COM',_binary '\0','Gnm19980521!','7K7KU2WRQAHWDH55UNW6KXJM2LRHRHAQ','5c3c18d5-6b42-488b-9b88-f3345f86304f',NULL,_binary '\0',_binary '\0',NULL,_binary '',0),('bff8c3cf-3f37-4fe2-b7ba-8a1dff81dcef','2222@qq.com','2222@QQ.COM','2222@qq.com','2222@QQ.COM',_binary '\0','Gnm19980521!','3KU47DO5PY5ZWF457VIXV2NJJ3FVOWXO','f7080bad-0915-4798-9189-458de5b39d2c',NULL,_binary '\0',_binary '\0',NULL,_binary '',0),('c46dda9e-702d-47e1-96e9-a09bae88ca17','dannike19980521@gmail.com','DANNIKE19980521@GMAIL.COM','dannike19980521@gmail.com','DANNIKE19980521@GMAIL.COM',_binary '\0','Gnm19980521!','4EZEQGG46N5JVAR4X22RHAAD3CZ5AARD','e6c7ac98-7117-4591-b4b3-477e9460a2f2',NULL,_binary '\0',_binary '\0',NULL,_binary '',0),('d61aa10e-0e6d-4928-bf55-feacd7e5be1d','281511301@qq.com','281511301@QQ.COM','281511301@qq.com','281511301@QQ.COM',_binary '\0','Gnm19980521!','YVABOJOJULP3CMV5YDLSCC2ACERBA2VS','70a71324-ab74-4c6c-9bda-6a001b732b50',NULL,_binary '\0',_binary '\0',NULL,_binary '',1),('e6b1bc6a-134b-4ef6-92eb-932d60896c51','dxk5418@psu.edu','DXK5418@PSU.EDU','dxk5418@psu.edu','DXK5418@PSU.EDU',_binary '\0','Gnm19980521!','JNBXPFXWY5V7VVS7SC573IB43G7EUSGV','f7e0f26a-4904-4570-b63f-5a046ea12b72',NULL,_binary '\0',_binary '\0',NULL,_binary '',0);
/*!40000 ALTER TABLE `aspnetusers` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2019-06-28 11:56:20
