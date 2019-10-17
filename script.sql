/*
SQLyog Professional v13.1.1 (64 bit)
MySQL - 5.7.27-0ubuntu0.18.04.1 : Database - congress
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`congress` /*!40100 DEFAULT CHARACTER SET utf8 */;

USE `congress`;

/*Table structure for table `category` */

DROP TABLE IF EXISTS `category`;

CREATE TABLE `category` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `parentCategoryId` int(11) DEFAULT '0',
  `name` varchar(50) DEFAULT NULL,
  `creatorId` int(11) DEFAULT '0',
  `creationDate` datetime DEFAULT CURRENT_TIMESTAMP,
  `statusId` tinyint(4) DEFAULT '2',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `category` */

/*Table structure for table `city` */

DROP TABLE IF EXISTS `city`;

CREATE TABLE `city` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `countryId` int(11) DEFAULT NULL,
  `name` varchar(50) DEFAULT NULL,
  `creatorId` int(11) DEFAULT '0',
  `creationDate` datetime DEFAULT CURRENT_TIMESTAMP,
  `statusId` tinyint(4) DEFAULT '2',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `city` */

/*Table structure for table `country` */

DROP TABLE IF EXISTS `country`;

CREATE TABLE `country` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) DEFAULT NULL,
  `creatorId` int(11) DEFAULT '0',
  `creationDate` datetime DEFAULT CURRENT_TIMESTAMP,
  `statusId` tinyint(4) DEFAULT '2',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `country` */

/*Table structure for table `event` */

DROP TABLE IF EXISTS `event`;

CREATE TABLE `event` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `userId` int(11) DEFAULT NULL,
  `name` varchar(100) DEFAULT NULL,
  `description` text,
  `logoPath` text,
  `countryId` int(11) DEFAULT NULL,
  `cityId` int(11) DEFAULT NULL,
  `address` text,
  `startDate` datetime DEFAULT CURRENT_TIMESTAMP,
  `endDate` datetime DEFAULT CURRENT_TIMESTAMP,
  `creatorId` int(11) DEFAULT '0',
  `creationDate` datetime DEFAULT CURRENT_TIMESTAMP,
  `statusId` tinyint(4) DEFAULT '2',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `event` */

/*Table structure for table `eventcategory` */

DROP TABLE IF EXISTS `eventcategory`;

CREATE TABLE `eventcategory` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `eventId` int(11) DEFAULT NULL,
  `categoryId` int(11) DEFAULT NULL,
  `creatorId` int(11) DEFAULT '0',
  `creationDate` datetime DEFAULT CURRENT_TIMESTAMP,
  `statusId` int(11) DEFAULT '2',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `eventcategory` */

/*Table structure for table `eventdetail` */

DROP TABLE IF EXISTS `eventdetail`;

CREATE TABLE `eventdetail` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `eventId` int(11) DEFAULT NULL,
  `startTime` time DEFAULT NULL,
  `endTime` time DEFAULT NULL,
  `day` int(11) DEFAULT NULL,
  `speakerName` varchar(50) DEFAULT NULL,
  `description` text,
  `creatorId` int(11) DEFAULT '0',
  `creationDate` datetime DEFAULT CURRENT_TIMESTAMP,
  `statusId` tinyint(4) DEFAULT '2',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `eventdetail` */

/*Table structure for table `eventparticipant` */

DROP TABLE IF EXISTS `eventparticipant`;

CREATE TABLE `eventparticipant` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `eventId` int(11) DEFAULT NULL,
  `userId` int(11) DEFAULT NULL,
  `creatorId` int(11) DEFAULT '0',
  `creationDate` datetime DEFAULT CURRENT_TIMESTAMP,
  `statusId` tinyint(4) DEFAULT '2',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `eventparticipant` */

/*Table structure for table `eventsponsor` */

DROP TABLE IF EXISTS `eventsponsor`;

CREATE TABLE `eventsponsor` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `eventId` int(11) DEFAULT NULL,
  `sponsorId` int(11) DEFAULT NULL,
  `creatorId` int(11) DEFAULT '0',
  `creationDate` datetime DEFAULT CURRENT_TIMESTAMP,
  `statusId` tinyint(4) DEFAULT '2',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `eventsponsor` */

/*Table structure for table `job` */

DROP TABLE IF EXISTS `job`;

CREATE TABLE `job` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) DEFAULT NULL,
  `creatorId` varchar(255) DEFAULT '0',
  `creationDate` datetime DEFAULT CURRENT_TIMESTAMP,
  `statusId` tinyint(4) DEFAULT '2',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `job` */

/*Table structure for table `menu` */

DROP TABLE IF EXISTS `menu`;

CREATE TABLE `menu` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `menuTypeId` tinyint(4) DEFAULT '1',
  `userTypeId` int(11) DEFAULT '1',
  `parentMenuId` int(11) DEFAULT '0',
  `priority` int(11) DEFAULT '0',
  `name` varchar(50) DEFAULT NULL,
  `icon` varchar(50) DEFAULT NULL,
  `path` varchar(50) DEFAULT NULL,
  `creatorId` int(11) DEFAULT '0',
  `creationDate` datetime DEFAULT CURRENT_TIMESTAMP,
  `statusId` tinyint(4) DEFAULT '2',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8;

/*Data for the table `menu` */

insert  into `menu`(`id`,`menuTypeId`,`userTypeId`,`parentMenuId`,`priority`,`name`,`icon`,`path`,`creatorId`,`creationDate`,`statusId`) values 
(1,1,1,0,1,'Anasayfa','fa fa-home','/Home',0,'2019-10-17 18:46:01',2),
(2,1,2,0,1,'Anasayfa','fa fa-home','/Home',0,'2019-10-17 18:46:10',2),
(3,1,3,0,1,'Anasayfa','fa fa-home','/Home',0,'2019-10-17 18:46:23',2),
(4,1,1,0,2,'Sponsorlar','fa fa-dollar-sign','/Sponsorlar',0,'2019-10-17 18:47:48',2),
(5,1,1,0,3,'Firmalar','fa fa-building','/Business',0,'2019-10-17 18:48:18',2),
(6,1,1,0,4,'Katılımcılar','fa fa-users','/Participant',0,'2019-10-17 18:48:31',2),
(7,1,1,0,6,'Profil Ayarları','fa fa-cogs','/Profile',0,'2019-10-17 18:49:13',2),
(8,1,2,0,4,'Profil Ayarları','fa fa-cogs','/Profile',0,'2019-10-17 18:49:29',2),
(9,1,3,0,3,'Profil Ayarları','fa fa-cogs','/Profile',0,'2019-10-17 18:49:53',2),
(10,1,2,0,2,'Etkinlik İşlemleri','fa fa-calendar','/Events',0,'2019-10-17 18:50:01',2),
(11,1,1,0,5,'Kategori Yönetimi','fa fa-ellipsis-h','/Categories',0,'2019-10-17 18:51:23',2),
(12,1,2,0,3,'Bildirim Yönetimi','fa fa-bullhorn','/SendNotification',0,'2019-10-17 18:54:03',2);

/*Table structure for table `sponsor` */

DROP TABLE IF EXISTS `sponsor`;

CREATE TABLE `sponsor` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) DEFAULT NULL,
  `logoPath` text,
  `creatorId` int(11) DEFAULT '0',
  `creationDate` datetime DEFAULT CURRENT_TIMESTAMP,
  `statusId` tinyint(4) DEFAULT '2',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `sponsor` */

/*Table structure for table `systemparameter` */

DROP TABLE IF EXISTS `systemparameter`;

CREATE TABLE `systemparameter` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `keystr` varchar(100) DEFAULT NULL,
  `valuestr` varchar(100) DEFAULT NULL,
  `creatorId` int(11) DEFAULT '0',
  `creationDate` datetime DEFAULT CURRENT_TIMESTAMP,
  `statusId` tinyint(4) DEFAULT '2',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `systemparameter` */

/*Table structure for table `user` */

DROP TABLE IF EXISTS `user`;

CREATE TABLE `user` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `userTypeId` tinyint(4) DEFAULT '3',
  `userGuid` varchar(128) DEFAULT NULL,
  `name` varchar(50) DEFAULT NULL,
  `surname` varchar(50) DEFAULT NULL,
  `email` varchar(100) DEFAULT NULL,
  `password` varchar(1024) DEFAULT NULL,
  `identityNr` varchar(11) DEFAULT NULL,
  `gender` tinyint(4) DEFAULT NULL,
  `birthDate` datetime DEFAULT CURRENT_TIMESTAMP,
  `jobId` int(11) DEFAULT NULL,
  `countryId` int(11) DEFAULT '0',
  `cityId` int(11) DEFAULT '1',
  `avatarPath` longtext,
  `phoneNr` varchar(15) DEFAULT NULL,
  `taxNr` varchar(15) DEFAULT NULL,
  `eventCount` int(11) DEFAULT '0',
  `emailVerification` tinyint(4) DEFAULT '1',
  `profileStatus` tinyint(4) DEFAULT '1',
  `notificationStatus` tinyint(4) DEFAULT '2',
  `creatorId` int(11) DEFAULT '0',
  `creationDate` datetime DEFAULT CURRENT_TIMESTAMP,
  `statusId` tinyint(4) DEFAULT '2',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `user` */

/*Table structure for table `userinterest` */

DROP TABLE IF EXISTS `userinterest`;

CREATE TABLE `userinterest` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `userId` int(11) DEFAULT NULL,
  `interestId` int(11) DEFAULT NULL,
  `creatorId` int(11) DEFAULT '0',
  `creationDate` datetime DEFAULT CURRENT_TIMESTAMP,
  `statusId` tinyint(4) DEFAULT '2',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `userinterest` */

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
