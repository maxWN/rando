DROP DATABASE IF EXISTS rando;
CREATE DATABASE IF NOT EXISTS rando;
USE rando;

SELECT 'CREATING DATABASE STRUCTURE' as 'INFO';

DROP TABLE IF EXISTS banks,
					 appliances,
					 blood_types,
					 users,
                     addresses,
                     credit_cards,
                     beers;
                     
CREATE TABLE IF NOT EXISTS `rando`.`banks` (
  `id` INT NOT NULL,
  `uid` VARCHAR(45) NULL,
  `account_number` VARCHAR(30) NOT NULL,
  `iban` VARCHAR(30) NULL,  
  `bank_name` VARCHAR(60) NOT NULL,
  `routing_number` VARCHAR(15) NOT NULL,
  `swift_bic` VARCHAR(15) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `uid_UNIQUE` (`uid` ASC) VISIBLE,
  UNIQUE INDEX `id_UNIQUE` (`id` ASC) VISIBLE)
COMMENT = 'Create table if doesn\'t already exist';

CREATE TABLE IF NOT EXISTS `rando`.`beers` (
  `id` INT NOT NULL,
  `uid` VARCHAR(40) NULL,
  `brand` VARCHAR(45) NULL,
  `name` VARCHAR(45) NULL,
  `style` VARCHAR(45) NULL,
  `hop` VARCHAR(20) NULL,
  `yeast` VARCHAR(20) NULL,
  `malts` VARCHAR(20) NULL,
  `ibu` VARCHAR(20) NULL,
  `alcohol` DECIMAL(10) NULL,
  `blg` VARCHAR(20) NULL,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `uid_UNIQUE` (`uid` ASC) VISIBLE);

INSERT INTO `rando`.`banks` VALUES (6539, 'e69fbf0a-ee5e-4a50-a980-ac32f7928cc2','9355841801','GB14ZVSH99972078064052','ABN AMRO FUND MANAGERS LIMITED','094928648','AACNGB21');
INSERT INTO `rando`.`beers` VALUES (5697,'878f2411-9bfa-472e-be79-027706e0dfa7','Leffe','Kirin Inchiban','Light Lager','Columbus','2308 - Munich Lager','Wheat mal','32 IBU',3.1,'18.4°Blg');
