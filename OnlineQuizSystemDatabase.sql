-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema OnlineQuizSystem
-- -----------------------------------------------------
DROP SCHEMA IF EXISTS `OnlineQuizSystem` ;

-- -----------------------------------------------------
-- Schema OnlineQuizSystem
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `OnlineQuizSystem` DEFAULT CHARACTER SET utf8 ;
USE `OnlineQuizSystem` ;

-- -----------------------------------------------------
-- Table `OnlineQuizSystem`.`Users`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `OnlineQuizSystem`.`Users` (
  `Phone` BIGINT NOT NULL,
  `FirstName` VARCHAR(16) NOT NULL,
  `LastName` VARCHAR(16) NOT NULL,
  `Sex` TINYINT(1) NULL,
  `Birth` INT NOT NULL,
  `EducationLevel` INT NOT NULL,
  `Program` VARCHAR(64) NULL,
  `Job` VARCHAR(64) NULL,
  `Registration` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `LastActivity` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Phone`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `OnlineQuizSystem`.`Questions`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `OnlineQuizSystem`.`Questions` (
  `QuestionID` INT NOT NULL AUTO_INCREMENT,
  `Path` VARCHAR(128) NOT NULL,
  `Category` INT NOT NULL,
  `Answer` INT NOT NULL,
  PRIMARY KEY (`QuestionID`),
  UNIQUE INDEX `soru_UNIQUE` (`Path` ASC) VISIBLE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `OnlineQuizSystem`.`Groups`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `OnlineQuizSystem`.`Groups` (
  `GroupID` INT NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(16) NOT NULL,
  `Link` VARCHAR(128) NOT NULL,
  `State` TINYINT(1) NOT NULL,
  `Pass` VARCHAR(16) NOT NULL,
  `RequiredScore` INT NOT NULL,
  PRIMARY KEY (`GroupID`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `OnlineQuizSystem`.`Messages`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `OnlineQuizSystem`.`Messages` (
  `MessageID` INT NOT NULL AUTO_INCREMENT,
  `Phone` BIGINT NOT NULL,
  `Content` VARCHAR(1024) NOT NULL,
  `SentDate` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Done` TINYINT(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`MessageID`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `OnlineQuizSystem`.`UserQuestions`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `OnlineQuizSystem`.`UserQuestions` (
  `UserQuestionID` INT NOT NULL AUTO_INCREMENT,
  `Phone` BIGINT NOT NULL,
  `Question` INT NOT NULL,
  `Answer` INT NOT NULL,
  `Duration` INT NOT NULL,
  PRIMARY KEY (`UserQuestionID`),
  INDEX `fk_userQuestions_users1_idx` (`Phone` ASC) VISIBLE,
  INDEX `fk_userQuestions_questions1_idx` (`Question` ASC) VISIBLE,
  UNIQUE INDEX `userques_unique` (`Phone` ASC, `Question` ASC) VISIBLE,
  CONSTRAINT `fk_userQuestions_users1`
    FOREIGN KEY (`Phone`)
    REFERENCES `OnlineQuizSystem`.`Users` (`Phone`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_userQuestions_questions1`
    FOREIGN KEY (`Question`)
    REFERENCES `OnlineQuizSystem`.`Questions` (`QuestionID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `OnlineQuizSystem`.`GlobalExceptionLogs`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `OnlineQuizSystem`.`GlobalExceptionLogs` (
  `ExceptionID` INT NOT NULL AUTO_INCREMENT,
  `Error` TEXT NULL,
  `detail` TEXT NULL,
  PRIMARY KEY (`ExceptionID`))
ENGINE = InnoDB;

USE `OnlineQuizSystem` ;

-- -----------------------------------------------------
-- Placeholder table for view `OnlineQuizSystem`.`QuestionsWithAnswers`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `OnlineQuizSystem`.`QuestionsWithAnswers` (`Question` INT, `CorrectAnswers` INT);

-- -----------------------------------------------------
-- procedure GetTopListRelative
-- -----------------------------------------------------

DELIMITER $$
USE `OnlineQuizSystem`$$
CREATE PROCEDURE `GetTopListRelative`() #Kullanıcılar bir soruyu cevapladığında, o soruyu doğru cevapladığında soru ne kadar az çözülmüşse o kadar çok puan alır. Yanlış cevapladığında ise soru ne kadar çok çözülmüşse o kadar çok puan kaybeder. Dolayısıyla skorlar göreceli olarak hesaplanmıştır.
BEGIN

SET SQL_SAFE_UPDATES = 0;

DROP TEMPORARY TABLE IF EXISTS QuestionRatios;
CREATE TEMPORARY TABLE QuestionRatios(`Id` INT PRIMARY KEY, `CorrectAnswers` int, `DirectRatio` double, `InverseRatio` double);

insert into QuestionRatios(`Id`, `CorrectAnswers`)
select Questions.QuestionId as Id, count(*) as CorrectAnswers 
from UserQuestions 
join Questions on UserQuestions.Question = Questions.QuestionId
where UserQuestions.Answer = Questions.Answer 
group by Questions.QuestionId; #select * from QuestionRatios;
											
set @count = (select count(*) from Questions); #select @count;
set @directRatio = @count/(select sum(CorrectAnswers/1) from QuestionRatios); #select @directRatio;																							
set @inverseRatio = @count/(select sum(1/CorrectAnswers) from QuestionRatios); #select @inverseRatio;
update QuestionRatios set DirectRatio = @directRatio*CorrectAnswers, InverseRatio = @inverseRatio/CorrectAnswers; #select * from QuestionRatios; 
																													
select Users.Phone, FirstName, concat(substring(LastName,1,1), ".") as "LastName", EducationLevel, Program,
#sum(case when Questions.Answer = UserQuestions.Answer then InverseRatio else 0 end) as a,
#sum(case when Questions.Answer != UserQuestions.Answer then  DirectRatio else 0 end) as b,
power(
	sum(case when Questions.Answer = UserQuestions.Answer then InverseRatio else 0 end)
    -
    sum(case when Questions.Answer != UserQuestions.Answer then DirectRatio else 0 end),
    
    2)/sum(UserQuestions.Duration/1000/60)*10 as Score
from Users
join UserQuestions on Users.Phone = UserQuestions.Phone
join Questions on Questions.QuestionId = UserQuestions.Question
join QuestionRatios on UserQuestions.Question = QuestionRatios.Id
where Users.Phone != 0
group by Users.Phone
order by Score desc;

SET SQL_SAFE_UPDATES = 1;
END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure GetPassives
-- -----------------------------------------------------

DELIMITER $$
USE `OnlineQuizSystem`$$
CREATE PROCEDURE `GetPassives`()
BEGIN
select concat("*",SUBSTRING(Phone, -4, 4)) as "Phone", 
FirstName, concat(substring(LastName,1,1), ".") as "LastName",
case
	when datediff(now(),LastActivity) > 10 then concat(datediff(now(),LastActivity)-8," days(*)")
	else concat(datediff(now(),LastActivity) - 8 , " days")
end
as time_out
from users where datediff(now(),LastActivity) between 8 and 22
order by LastActivity desc;
END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure GetNotPassed
-- -----------------------------------------------------

DELIMITER $$
USE `OnlineQuizSystem`$$
CREATE PROCEDURE `GetNotPassed`(IN phones VARCHAR(1024))
BEGIN
DROP TEMPORARY TABLE if exists tempwp;
CREATE TEMPORARY TABLE tempwp(`phone` BIGINT UNSIGNED PRIMARY KEY);
set @q = concat("insert into tempwp values", phones, ";");
PREPARE stmt FROM @q;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;
select * from tempwp where no not in (select users.phone from users);
END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure GetTopListStatic
-- -----------------------------------------------------

DELIMITER $$
USE `OnlineQuizSystem`$$
CREATE PROCEDURE `GetTopListStatic`() #Puan netlerin karesinin geçen zamana bölümünün, ideal puana oranıyla hesaplanmaktadır.
BEGIN
set @maxScore =  10.0 * 10.0 / 100000.0; #ideal puan
select FirstName,
concat(substring(LastName,1,1), ".") as "LastName", 
EducationLevel,
Program,
Job,
pow(sum(UserQuestions.Answer = Questions.Answer) - sum(UserQuestions.Answer != Questions.Answer) ,2)/sum(Duration)/@maxScore*100 as Score
from `Users` 
join UserQuestions on UserQuestions.Phone = Users.Phone
join Questions on Questions.QuestionID = UserQuestions.Question
where Users.Phone != 0
group by Users.Phone
order by Score desc;
END$$

DELIMITER ;

-- -----------------------------------------------------
-- View `OnlineQuizSystem`.`QuestionsWithAnswers`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `OnlineQuizSystem`.`QuestionsWithAnswers`;
USE `OnlineQuizSystem`;
CREATE  OR REPLACE VIEW `QuestionsWithAnswers` AS
select Question, sum(Questions.Answer = UserQuestions.Answer) as CorrectAnswers
from userQuestions
join Questions on Questions.QuestionID = UserQuestions.Question
group by question;

SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;

-- -----------------------------------------------------
-- Data for table `OnlineQuizSystem`.`Users`
-- -----------------------------------------------------
START TRANSACTION;
USE `OnlineQuizSystem`;
INSERT INTO `OnlineQuizSystem`.`Users` (`Phone`, `FirstName`, `LastName`, `Sex`, `Birth`, `EducationLevel`, `Program`, `Job`, `Registration`, `LastActivity`) VALUES (0, ' ', ' ', 0, 0, 0, ' ', NULL, DEFAULT, DEFAULT);

COMMIT;


-- -----------------------------------------------------
-- Data for table `OnlineQuizSystem`.`Groups`
-- -----------------------------------------------------
START TRANSACTION;
USE `OnlineQuizSystem`;
INSERT INTO `OnlineQuizSystem`.`Groups` (`GroupID`, `Name`, `Link`, `State`, `Pass`, `RequiredScore`) VALUES (1, 'academia', '...', 1, 'SHA ile şifrelenecek', 10);

COMMIT;

-- begin attached script 'questions'
INSERT INTO `questions` (`QuestionID`, `Path`, `Category`, `Answer`) VALUES
(1, '\\Questions\\1-Rasyonel\\10.png', 1, 2),
(2, '\\Questions\\1-Rasyonel\\1.png', 1, 3),
(3, '\\Questions\\1-Rasyonel\\2.png', 1, 2),
(4, '\\Questions\\1-Rasyonel\\3.png', 1, 0),
(5, '\\Questions\\1-Rasyonel\\4.png', 1, 2),
(6, '\\Questions\\1-Rasyonel\\5.png', 1, 3),
(7, '\\Questions\\1-Rasyonel\\6.png', 1, 2),
(8, '\\Questions\\1-Rasyonel\\7.png', 1, 4),
(9, '\\Questions\\1-Rasyonel\\8.png', 1, 4),
(10, '\\Questions\\1-Rasyonel\\9.png', 1, 0),
(11, '\\Questions\\10-Carpanlara Ayirma\\10.png', 10, 4),
(12, '\\Questions\\10-Carpanlara Ayirma\\1.png', 10, 3),
(13, '\\Questions\\10-Carpanlara Ayirma\\2.png', 10, 2),
(14, '\\Questions\\10-Carpanlara Ayirma\\3.png', 10, 0),
(15, '\\Questions\\10-Carpanlara Ayirma\\4.png', 10, 4),
(16, '\\Questions\\10-Carpanlara Ayirma\\5.png', 10, 3),
(17, '\\Questions\\10-Carpanlara Ayirma\\6.png', 10, 3),
(18, '\\Questions\\10-Carpanlara Ayirma\\7.png', 10, 4),
(19, '\\Questions\\10-Carpanlara Ayirma\\8.png', 10, 0),
(20, '\\Questions\\10-Carpanlara Ayirma\\9.png', 10, 1),
(21, '\\Questions\\2-Basamak Kavrami\\10.png', 2, 4),
(22, '\\Questions\\2-Basamak Kavrami\\1.png', 2, 2),
(23, '\\Questions\\2-Basamak Kavrami\\2.png', 2, 1),
(24, '\\Questions\\2-Basamak Kavrami\\3.png', 2, 3),
(25, '\\Questions\\2-Basamak Kavrami\\4.png', 2, 0),
(26, '\\Questions\\2-Basamak Kavrami\\5.png', 2, 3),
(27, '\\Questions\\2-Basamak Kavrami\\6.png', 2, 1),
(28, '\\Questions\\2-Basamak Kavrami\\7.png', 2, 3),
(29, '\\Questions\\2-Basamak Kavrami\\8.png', 2, 3),
(30, '\\Questions\\2-Basamak Kavrami\\9.png', 2, 0),
(31, '\\Questions\\3-Bolme Bolunebilme\\10.png', 3, 1),
(32, '\\Questions\\3-Bolme Bolunebilme\\1.png', 3, 2),
(33, '\\Questions\\3-Bolme Bolunebilme\\2.png', 3, 3),
(34, '\\Questions\\3-Bolme Bolunebilme\\3.png', 3, 2),
(35, '\\Questions\\3-Bolme Bolunebilme\\4.png', 3, 0),
(36, '\\Questions\\3-Bolme Bolunebilme\\5.png', 3, 0),
(37, '\\Questions\\3-Bolme Bolunebilme\\6.png', 3, 3),
(38, '\\Questions\\3-Bolme Bolunebilme\\7.png', 3, 4),
(39, '\\Questions\\3-Bolme Bolunebilme\\8.png', 3, 4),
(40, '\\Questions\\3-Bolme Bolunebilme\\9.png', 3, 4),
(41, '\\Questions\\4-Faktoriyel\\10.png', 4, 3),
(42, '\\Questions\\4-Faktoriyel\\1.png', 4, 1),
(43, '\\Questions\\4-Faktoriyel\\2.png', 4, 0),
(44, '\\Questions\\4-Faktoriyel\\3.png', 4, 2),
(45, '\\Questions\\4-Faktoriyel\\4.png', 4, 4),
(46, '\\Questions\\4-Faktoriyel\\5.png', 4, 4),
(47, '\\Questions\\4-Faktoriyel\\6.png', 4, 3),
(48, '\\Questions\\4-Faktoriyel\\7.png', 4, 4),
(49, '\\Questions\\4-Faktoriyel\\8.png', 4, 2),
(50, '\\Questions\\4-Faktoriyel\\9.png', 4, 3),
(51, '\\Questions\\5-Mutlak Deger\\10.png', 5, 1),
(52, '\\Questions\\5-Mutlak Deger\\1.png', 5, 0),
(53, '\\Questions\\5-Mutlak Deger\\2.png', 5, 1),
(54, '\\Questions\\5-Mutlak Deger\\3.png', 5, 3),
(55, '\\Questions\\5-Mutlak Deger\\4.png', 5, 4),
(56, '\\Questions\\5-Mutlak Deger\\5.png', 5, 0),
(57, '\\Questions\\5-Mutlak Deger\\6.png', 5, 4),
(58, '\\Questions\\5-Mutlak Deger\\7.png', 5, 0),
(59, '\\Questions\\5-Mutlak Deger\\8.png', 5, 1),
(60, '\\Questions\\5-Mutlak Deger\\9.png', 5, 2),
(61, '\\Questions\\6-Birinci Dereceden Denklemler\\10.png', 6, 0),
(62, '\\Questions\\6-Birinci Dereceden Denklemler\\1.png', 6, 3),
(63, '\\Questions\\6-Birinci Dereceden Denklemler\\2.png', 6, 1),
(64, '\\Questions\\6-Birinci Dereceden Denklemler\\3.png', 6, 4),
(65, '\\Questions\\6-Birinci Dereceden Denklemler\\4.png', 6, 0),
(66, '\\Questions\\6-Birinci Dereceden Denklemler\\5.png', 6, 3),
(67, '\\Questions\\6-Birinci Dereceden Denklemler\\6.png', 6, 3),
(68, '\\Questions\\6-Birinci Dereceden Denklemler\\7.png', 6, 4),
(69, '\\Questions\\6-Birinci Dereceden Denklemler\\8.png', 6, 1),
(70, '\\Questions\\6-Birinci Dereceden Denklemler\\9.png', 6, 4),
(71, '\\Questions\\7-Basit Esitsizlikler\\10.png', 7, 3),
(72, '\\Questions\\7-Basit Esitsizlikler\\1.png', 7, 0),
(73, '\\Questions\\7-Basit Esitsizlikler\\2.png', 7, 1),
(74, '\\Questions\\7-Basit Esitsizlikler\\3.png', 7, 4),
(75, '\\Questions\\7-Basit Esitsizlikler\\4.png', 7, 1),
(76, '\\Questions\\7-Basit Esitsizlikler\\5.png', 7, 4),
(77, '\\Questions\\7-Basit Esitsizlikler\\6.png', 7, 2),
(78, '\\Questions\\7-Basit Esitsizlikler\\7.png', 7, 3),
(79, '\\Questions\\7-Basit Esitsizlikler\\8.png', 7, 1),
(80, '\\Questions\\7-Basit Esitsizlikler\\9.png', 7, 1),
(81, '\\Questions\\8-Koklu Sayilar\\10.png', 8, 3),
(82, '\\Questions\\8-Koklu Sayilar\\1.png', 8, 3),
(83, '\\Questions\\8-Koklu Sayilar\\2.png', 8, 4),
(84, '\\Questions\\8-Koklu Sayilar\\3.png', 8, 1),
(85, '\\Questions\\8-Koklu Sayilar\\4.png', 8, 1),
(86, '\\Questions\\8-Koklu Sayilar\\5.png', 8, 3),
(87, '\\Questions\\8-Koklu Sayilar\\6.png', 8, 2),
(88, '\\Questions\\8-Koklu Sayilar\\7.png', 8, 3),
(89, '\\Questions\\8-Koklu Sayilar\\8.png', 8, 2),
(90, '\\Questions\\8-Koklu Sayilar\\9.png', 8, 4),
(91, '\\Questions\\9-Uslu Sayilar\\10.png', 9, 3),
(92, '\\Questions\\9-Uslu Sayilar\\1.png', 9, 3),
(93, '\\Questions\\9-Uslu Sayilar\\2.png', 9, 4),
(94, '\\Questions\\9-Uslu Sayilar\\3.png', 9, 1),
(95, '\\Questions\\9-Uslu Sayilar\\4.png', 9, 1),
(96, '\\Questions\\9-Uslu Sayilar\\5.png', 9, 2),
(97, '\\Questions\\9-Uslu Sayilar\\6.png', 9, 3),
(98, '\\Questions\\9-Uslu Sayilar\\7.png', 9, 1),
(99, '\\Questions\\9-Uslu Sayilar\\8.png', 9, 2),
(100, '\\Questions\\9-Uslu Sayilar\\9.png', 9, 3);

-- end attached script 'questions'
-- begin attached script 'userQuestions'
insert into UserQuestions(Phone, Question, Answer, Duration)
select 0 as Phone, Questions.QuestionId, Questions.Answer, 0 as Duration
from Questions
-- end attached script 'userQuestions'
