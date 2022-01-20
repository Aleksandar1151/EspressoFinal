-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------
-- -----------------------------------------------------
-- Schema espressodb
-- -----------------------------------------------------
DROP SCHEMA IF EXISTS `espressodb` ;

-- -----------------------------------------------------
-- Schema espressodb
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `espressodb` DEFAULT CHARACTER SET utf8 ;
USE `espressodb` ;

-- -----------------------------------------------------
-- Table `espressodb`.`kategorija`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `espressodb`.`kategorija` (
  `idKategorija` INT NOT NULL AUTO_INCREMENT,
  `naziv` VARCHAR(45) NULL DEFAULT NULL,
  `boja` VARCHAR(45) NULL DEFAULT NULL,
  PRIMARY KEY (`idKategorija`))
ENGINE = InnoDB
AUTO_INCREMENT = 3
DEFAULT CHARACTER SET = utf8mb3;


-- -----------------------------------------------------
-- Table `espressodb`.`artikal`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `espressodb`.`artikal` (
  `idArtikal` INT NOT NULL AUTO_INCREMENT,
  `naziv` VARCHAR(45) NULL DEFAULT NULL,
  `cijena` VARCHAR(45) NULL DEFAULT NULL,
  `kolicina` VARCHAR(45) NULL DEFAULT NULL,
  `Kategorija_idKategorija` INT NOT NULL,
  PRIMARY KEY (`idArtikal`, `Kategorija_idKategorija`),
  CONSTRAINT `fk_Artikal_Kategorija1`
    FOREIGN KEY (`Kategorija_idKategorija`)
    REFERENCES `espressodb`.`kategorija` (`idKategorija`))
ENGINE = InnoDB
AUTO_INCREMENT = 7
DEFAULT CHARACTER SET = utf8mb3;

CREATE INDEX `fk_Artikal_Kategorija1_idx` ON `espressodb`.`artikal` (`Kategorija_idKategorija` ASC) VISIBLE;


-- -----------------------------------------------------
-- Table `espressodb`.`nalog`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `espressodb`.`nalog` (
  `idNalog` INT NOT NULL AUTO_INCREMENT,
  `ime` VARCHAR(45) NULL DEFAULT NULL,
  `lozinka` VARCHAR(45) NULL DEFAULT NULL,
  `privilegije` TINYINT NULL DEFAULT NULL,
  PRIMARY KEY (`idNalog`))
ENGINE = InnoDB
AUTO_INCREMENT = 3
DEFAULT CHARACTER SET = utf8mb3;


-- -----------------------------------------------------
-- Table `espressodb`.`otpis`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `espressodb`.`otpis` (
  `idOtpis` INT NOT NULL AUTO_INCREMENT,
  `kolicina` VARCHAR(45) NULL DEFAULT NULL,
  `Nalog_idNalog` INT NOT NULL,
  `Artikal_idArtikal` INT NOT NULL,
  PRIMARY KEY (`idOtpis`),
  CONSTRAINT `fk_Otpis_Artikal1`
    FOREIGN KEY (`Artikal_idArtikal`)
    REFERENCES `espressodb`.`artikal` (`idArtikal`),
  CONSTRAINT `fk_Otpis_Nalog1`
    FOREIGN KEY (`Nalog_idNalog`)
    REFERENCES `espressodb`.`nalog` (`idNalog`))
ENGINE = InnoDB
AUTO_INCREMENT = 3
DEFAULT CHARACTER SET = utf8mb3;

CREATE INDEX `fk_Otpis_Nalog1_idx` ON `espressodb`.`otpis` (`Nalog_idNalog` ASC) VISIBLE;

CREATE INDEX `fk_Otpis_Artikal1_idx` ON `espressodb`.`otpis` (`Artikal_idArtikal` ASC) VISIBLE;


-- -----------------------------------------------------
-- Table `espressodb`.`racun`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `espressodb`.`racun` (
  `idRacun` INT NOT NULL AUTO_INCREMENT,
  `datum` VARCHAR(45) NULL DEFAULT NULL,
  `iznos` VARCHAR(45) NULL DEFAULT NULL,
  `Nalog_idNalog` INT NOT NULL,
  PRIMARY KEY (`idRacun`, `Nalog_idNalog`),
  CONSTRAINT `fk_Racun_Nalog1`
    FOREIGN KEY (`Nalog_idNalog`)
    REFERENCES `espressodb`.`nalog` (`idNalog`))
ENGINE = InnoDB
AUTO_INCREMENT = 20
DEFAULT CHARACTER SET = utf8mb3;

CREATE INDEX `fk_Racun_Nalog1_idx` ON `espressodb`.`racun` (`Nalog_idNalog` ASC) VISIBLE;


-- -----------------------------------------------------
-- Table `espressodb`.`storniranracun`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `espressodb`.`storniranracun` (
  `idStorniranRacun` INT NOT NULL AUTO_INCREMENT,
  `Racun_idRacun` INT NOT NULL,
  PRIMARY KEY (`idStorniranRacun`),
  CONSTRAINT `fk_StorniranRacun_Racun1`
    FOREIGN KEY (`Racun_idRacun`)
    REFERENCES `espressodb`.`racun` (`idRacun`))
ENGINE = InnoDB
AUTO_INCREMENT = 10
DEFAULT CHARACTER SET = utf8mb3;

CREATE INDEX `fk_StorniranRacun_Racun1_idx` ON `espressodb`.`storniranracun` (`Racun_idRacun` ASC) VISIBLE;


-- -----------------------------------------------------
-- Table `espressodb`.`stavka`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `espressodb`.`stavka` (
  `Racun_idRacun` INT NOT NULL,
  `Artikal_idArtikal` INT NOT NULL,
  `naziv` VARCHAR(45) NULL DEFAULT NULL,
  `kolicina` VARCHAR(45) NULL DEFAULT NULL,
  `cijena` VARCHAR(45) NULL DEFAULT NULL,
  `StorniranRacun_idStorniranRacun` INT NULL DEFAULT NULL,
  CONSTRAINT `fk_Racun_has_Artikal_Artikal1`
    FOREIGN KEY (`Artikal_idArtikal`)
    REFERENCES `espressodb`.`artikal` (`idArtikal`),
  CONSTRAINT `fk_Racun_has_Artikal_Racun`
    FOREIGN KEY (`Racun_idRacun`)
    REFERENCES `espressodb`.`racun` (`idRacun`),
  CONSTRAINT `fk_Stavka_StorniranRacun1`
    FOREIGN KEY (`StorniranRacun_idStorniranRacun`)
    REFERENCES `espressodb`.`storniranracun` (`idStorniranRacun`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb3;

CREATE INDEX `fk_Racun_has_Artikal_Artikal1_idx` ON `espressodb`.`stavka` (`Artikal_idArtikal` ASC) VISIBLE;

CREATE INDEX `fk_Racun_has_Artikal_Racun_idx` ON `espressodb`.`stavka` (`Racun_idRacun` ASC) VISIBLE;

CREATE INDEX `fk_Stavka_StorniranRacun1_idx` ON `espressodb`.`stavka` (`StorniranRacun_idStorniranRacun` ASC) VISIBLE;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
