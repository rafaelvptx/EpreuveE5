<?PHP
Function connex($base,$param){
include_once($param.".inc.php");
$idconnex = @mysql_connect(HOST,LOGIN,MDP);
$idbase = @mysql_select_db($base);
return $idconnex;
}
?>
