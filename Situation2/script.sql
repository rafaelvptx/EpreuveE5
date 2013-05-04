-- phpMyAdmin SQL Dump
-- version 2.6.4-pl4
-- http://www.phpmyadmin.net
-- 
-- Serveur: localhost
-- Généré le : Jeudi 03 Mai 2012 à 15:59
-- Version du serveur: 5.0.18
-- Version de PHP: 4.3.11
-- 
-- Base de données: `election`
-- 

-- --------------------------------------------------------

-- 
-- Structure de la table `classe`
-- 

CREATE TABLE `classe` (
  `codeClasse` varchar(10) NOT NULL,
  PRIMARY KEY(codeClasse)
);


-- 
-- Structure de la table `lyceen`
-- 

CREATE TABLE lyceen (
  id varchar(20) NOT NULL,
  nom varchar(20),
  prenom varchar(20),
  mdp varchar(20) NOT NULL,
  voter datetime,
  idClasse varchar(10),
  PRIMARY KEY(id),
  FOREIGN KEY(idClasse) REFERENCES classe(codeClasse)
 
);

-- 
-- Structure de la table parametre
-- 

CREATE TABLE parametre (
  debutElect datetime,
  finElect datetime,
  voteNull integer(4),
  voteBlanc integer(4)
);

-- 
-- Contenu de la table `parametre`
-- 

-- --------------------------------------------------------

--
-- Structure de la table candidat
--

CREATE TABLE candidat (
  idCandidat varchar(20) NOT NULL,
	professionFoi varchar(100),
	nbVoix integer(4),
	idSupp varchar(20) NOT NULL,
	PRIMARY KEY(idCandidat),
	FOREIGN KEY(idCandidat) REFERENCES lyceen(id)
	);

	
CREATE TABLE suppleant(
	idSupp varchar(20) NOT NULL,
	PRIMARY KEY(idSupp),
	FOREIGN KEY(idSupp) REFERENCES lyceen(id)
	);
	
	
ALTER TABLE candidat
ADD FOREIGN KEY(idSupp) REFERENCES suppleant(idSupp);
