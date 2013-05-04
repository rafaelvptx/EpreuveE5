<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN" "http://www.w3.org/TR/html4/strict.dtd">
<html>
<head>
  <title>Authentification</title>
	<script type="text/JavaScript" src="script.js">
	</script>
	<style type="text/css">
			@import url(authentification.css);
		</style>
	</head>
<body>
	<form action = '#' method='post' name='form'>
		

<?php
//inclusion des paramètres de connexion
include_once("connex.inc.php");

//appel de la fonction php avec base : election et utilisateur : codeConnexion 
$idconnex = connex("election","codeConnexion");

//envoie de la requete pour recupérer la date et l'heure de debut et de fin des elections
$requete = "select debutElect, finElect from parametre";
$result = @mysql_query($requete,$idconnex);
if($result){

$tab = @mysql_fetch_array($result);
if($tab != ""){

//Création du timestamp pour les dates début et date de fin
$tmp = explode(' ',$tab[0]);
$date = explode('-',$tmp[0]);
$heure = explode(':',$tmp[1]);
$debut = mktime ($heure[0] , $heure[1] , $heure[2] , $date[1] , $date[2] , $date[0] );

//Récupération de la date et l'heure contenu en paramètre dans la base
$tmp = explode(' ',$tab[1]);
$date = explode('-',$tmp[0]);
$heure = explode(':',$tmp[1]);
$fin = mktime ($heure[0] , $heure[1] , $heure[2] , $date[1] , $date[2] , $date[0] );

//Timestamp de la date et de l'heure actuelle
$maintenant = time();

//Compare les trois times stamps pour savoir si le vote est ouvert ou non 
if($maintenant > $debut && $maintenant < $fin){ 
	
	//Si l'utilisateur a déjà écrit son id et son mdp
	if(isset($_POST['id']) && isset($_POST['mdp'])){
		//Envoie de la requete permettant de savoir si le lycéen existe bien
		$requete = "select nom from lyceen where id = \"".$_POST['id']."\" and mdp = \"".$_POST['mdp']."\"";
		$result = @mysql_query($requete,$idconnex);
		$tab = @mysql_fetch_array($result);
		
		//Si le lycéen n'existe pas la BDD n'envera rien
		if($tab[0]==""){
			
			//inclusion de la page de connexion
			include_once("authentification1.html");
			echo "<SCRIPT language='Javascript'>document.getElementById('erreur').innerHTML = 'Login ou mot de passe incorrect'</SCRIPT>";
		
		}
		else{
			$requete = "select voter from lyceen where id = \"".$_POST['id']."\" and mdp = \"".$_POST['mdp']."\"";
			$result = @mysql_query($requete,$idconnex);
			$tab = @mysql_fetch_array($result);
			if($tab[0]==""){
				session_start();
				$_SESSION['id'] = $_POST['id'];
				header('Location: Vote.php');
				$_SESSION['id2'] = $_POST['id'];
			}
			else{
				include("erreur.html");
			}
			
		}

	}
	else{
	include_once("authentification1.html");
	}
}
else{
	include("authentification2.html");
	}
}
else{
	echo "Connexion impossible par manque des heures de vote dans la base";
}
}

else{
echo "Erreur de connexion a la base de donnée";
}
?>

	</form>
</body>
</html>
