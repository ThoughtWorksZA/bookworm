#To run this script,
# 1. Create a log directory
# 2. Put the export.csv file under log directory. 
#    The export.csv is exported from Raven's books document. It should include CoverImageUrl column.
# 3. Run the script
# 4. Check the result. The books with wrong image urls are stored in log/log.txt. 
#    The books' correct images are all downloaded to log/images_all
 
$baseDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$logFile = "$baseDir\log.txt"
$imagesPath = "$baseDir\images_all"
$csv = "$baseDir\Export.csv"

$urls = @()
$books = @()
$failedBooks = @()

If (Test-Path $imagesPath){
    Remove-Item -Recurse -Force $imagesPath
}

If (Test-Path $logFile){
    Remove-Item $logFile
}

mkdir $imagesPath

import-csv $csv|`
        ForEach-Object {
            $urls += $_.CoverImageUrl
            $books += $_.Id
        }

$total = $urls.length
write-host "$total in total"

$index = 1
foreach ($url in $urls) {
    write-host "$index"
    try {
        $imageName = ($books[$index-1] -replace '/', '_')
        (New-Object System.Net.WebClient).DownloadFile($url, "$imagesPath\$imageName.jpg")
    } catch [Exception]{
        $failedBooks += $books[$index-1] -replace 'books/', 'http://www.puku.co.za/Books/Details/'
    }
    $index++
}

$failedBooks | out-file $logFile