
function searchbytag(tag)
{
   
    var temptag;
   
    var hashtest = tag.substring(0, 1);
    if (hashtest === '#') 
        temptag = tag.substring(1);
     else
        temptag = tag;

    window.location = `http://localhost:56490/home/index?SearchType=3&SearchArg=${temptag}`;
 
}