<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml-strict.dtd">
<html>
<head>
<style type = "text/css">
  	@import url(ccsphpVote.css);
</style>
</head>
<body>

<?php
	session_start();
	if(isset($_SESSION['id2'])){
	$id = $_SESSION['id2'];
	unset($_SESSION['id2']);
	
	define("HOST","localhost");
	define("LOGIN","root");
	define("MDP","");
	$base = "election";

	//Connexion au serveur
	$idconnex = @mysql_connect(HOST, LOGIN, MDP);

	//Sélection de la base de données
	$idbase = @mysql_select_db($base);


	if(isset($_POST['candidat'])){
		$candidat = $_POST['candidat'];
		$taille = count($candidat);
		
		if($taille > 5){
			$requete = "UPDATE Parametre SET voteNull = voteNull + 1";
			$result = mysql_query($requete);
		}
		
		else{
			for($i=0;$i<$taille;$i++){
				$requete = "UPDATE Candidat SET nbVoix = nbVoix + 1 WHERE idCandidat='".$candidat[$i]."'";
				$result = mysql_query($requete);
			}
		}
	}
		
	else{	
		$requete = "UPDATE Parametre SET voteBlanc = voteBlanc + 1";
		$result = mysql_query($requete);
	}
	
	$requete = "UPDATE lyceen SET voter = '".date('Y-m-j H:i:s')."' WHERE id='".$id."';";
	$result = mysql_query($requete);
	}
	else{
		header('Location: debut.php');
		}
	
?>

</body>
</html>
