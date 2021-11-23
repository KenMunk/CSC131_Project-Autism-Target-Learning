$rawPictures = Get-ChildItem -Force pexels-* -Name
$originalPath = pwd
Write-Host "Original Path detected $originalPath"
Write-Host "Untagged pictures found:"
foreach($picture in $rawPictures){
    
    Write-Host $picture


}

if($rawPictures.Count -gt 0){
    $newTag = Read-Host "Please enter your desired tag: "
    foreach($picture in $rawPictures){
        
        Write-Host "$originalPath\$newTag--$picture"
        Rename-Item -Path "$originalPath\$picture" -NewName "$newTag--$picture"

    }
}

