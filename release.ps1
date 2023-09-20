Write-Output "Did you check..."
Read-Host

$version = $args[0]

git add .
git commit -m "build: ${version}"
git tag -m "This is Updater build ${version}" "${version}"
git push --atomic origin master ${version}