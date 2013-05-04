<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml-strict.dtd">
<html>
<head>
<title>Vote en ligne</title>
<style type = "text/css">
  	@import url(ccsphpVote.css);
</style>
<script type="text/JavaScript" src="votescript.js">
	</script>
</head>

<body onload='cacher()'>
<form method='post' action='Fin.php'>
<?php
session_start();

//récupération de l'id du lycéen
if(isset($_SESSION['id'])){
	$id = $_SESSION['id'];
	unset($_SESSION['id']);
	
	}
else{
	header('Location: debut.php');}

//paramètre de connexion 
define("HOST","localhost");
define("LOGIN","root");
define("MDP","");
$base = "election";

//Connexion au serveur
$idconnex = @mysql_connect(HOST, LOGIN, MDP);

//Sélection de la base de données
$idbase = @mysql_select_db($base);


?>
<div id="haut">
	<?php
	$requete='select nom, prenom from lyceen where id=\''.$id.'\';';
	$result = mysql_query($requete);
	$ligne = mysql_fetch_array($result);
	
	echo $ligne[0].' '.$ligne[1];
	
	?>
</div>

<div id="conteneurPrincipal">
<fieldset id= "tdh1"> <legend id= "tdh2">Contexte de l'élection</legend>
	<p>Chers élèves,</p><br/>
	<p>Chaque année, le Conseil pour la Vie Lycéenne est renouvelé par moitié, soit 5 sièges à pourvoir.
	Le vote se fait au suffrage universel direct, c’est-à-dire que tous les lycéens et étudiants du lycée sont électeurs et éligibles, soit environ 2800 personnes.<br/><br/> 
	</p>
	<ul>
	<li>Il se fait au scrutin plurinominal à 1 tour.</li>
	<li>Vous avez la possibilité de voter pour 5 personnes maximum.</li>
	<li>Au-delà, le vote est considéré comme nul.</li>
	<li>Vous pouvez, en revanche, ne voter que pour 1, 2, 3 ou 4 personnes.</li>
	<li>Vous avez également la possibilité de voter « blanc » en ne cochant aucun candidat.</li></ul><br/><br/>
	<label id='accepter'><input type='checkbox' onclick='montrer()'> <b><u> J'ai lu et j'accepte les conditions de vote </u></b> </label>
</fieldset>
<p></p>

<div id="conteneur">
	<div id="candidat">
		<span>Candidats :</span><br/><br/>
		
<?php
			//affichage des candidates dans la page php
			$marequête = "select nom, prenom, idSupp, id from lyceen, candidat where candidat.idCandidat =  lyceen.id and id in (select idCandidat from candidat)";
			$resultat = mysql_query($marequête);
			$trouveResult=False;
			if ($resultat)
			{
				$i=0;
				echo "<table border = '1'>";
				while ($ligne = mysql_fetch_array($resultat))
				{
					$trouveResult=True;
					echo "<p>";
					echo "<input type = checkbox id='checkbox' name='candidat[]' value='".$ligne[3]."'>";
					echo "<a id='click' onclick='afficherProfession()' id='".$ligne[3]."'>" . $ligne[0] . " " .$ligne[1] . "</a>";
					$suppl[$i] = $ligne[2];
					echo "<p>";
					$i++;
				}
				echo "</table>";
			
			}
			
?>
	</div>

	<div id="suppleant">
		<span>Suppléants associés :</span><br/><br/>
<?php
	if($trouveResult){
	$taille = count($suppl);
	
	$i = 0;
	
	while ($i < $taille)
	{
		$marequête = "select nom, prenom from lyceen where id = '".$suppl[$i]."'";
		$resultat = mysql_query($marequête);
		$ligne = mysql_fetch_array($resultat);
		
		echo "<p>";
		echo  $ligne[0] . " " .$ligne[1];
		echo "<p>";
		$i ++;
	}
	}
?>
	</div>

</div>
<div id="bouton">
<br></br>
<input type = submit id="submit" value='Voter' >
</div>
</div>
</form>
</body>
</html>
