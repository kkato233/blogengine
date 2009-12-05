CREATE TABLE IF NOT EXISTS `be_blogrollitems` (
  `BlogRollID` varchar(36) NOT NULL,
  `Title` varchar(255) NOT NULL,
  `Description` longtext DEFAULT NULL,
  `BlogUrl` varchar(255) NOT NULL,
  `FeedUrl` varchar(255) DEFAULT NULL,
  `Xfn` varchar(255) DEFAULT NULL,
  `SortIndex` int(10) NOT NULL,
  PRIMARY KEY  (`BlogRollID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS `be_referrers` (
  `ReferrerId` varchar(36) NOT NULL,
  `ReferralDay` datetime NOT NULL,
  `ReferrerUrl` varchar(255) NOT NULL,
  `ReferralCount` int(10) NOT NULL,
  `Url` varchar(255) DEFAULT NULL,
  `IsSpam` tinyint(1) NULL,
  PRIMARY KEY  (`ReferrerId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

ALTER TABLE `be_pages` ADD `Slug` VARCHAR(255) DEFAULT NULL;
ALTER TABLE `be_postcomment` ADD `ModeratedBy` VARCHAR(100) DEFAULT NULL;
