@echo off
:: Print start time
echo Started at -------------------------- %time% --------------------------
:: Using utf-8 in CMD
chcp 65001 >nul 2>&1


:: ------------------------------- VARIABLES -------------------------------
:: please override everythings in <> """ not anything else """
:: Example: set "sln=<YOUR_SOLUTION_NAME>" => set "sln=MoeinWeb.sln"

:: Output folder name
set "output=output"

:: Compile folder name
set "compileFolder=compile\WooCommerceApi"

:: Solution name
set "sln=WooCommerceApi.sln"

:: Biuld configuration name, default is "Compile"
set "buildConfigurationName=Compile"

:: Compile path
set "compileOutput=%cd%\%output%\%compileFolder%"

:: ----------------------------- END_VARIABLES -----------------------------


:: ------------------------ INFORMATION_VALIDATION -------------------------

echo output folder name: %output%
echo compile folder name: %compileFolder%
echo solution name: %sln%
echo build configuration name: %buildConfigurationName%
echo compile output: %compileOutput%

echo:
echo "The above information correct? If you make a mistake, exit the program"
pause

:: ----------------------- END_INFORMATION_VALIDATION ----------------------


:: ----------------------------- CREATE_FOLDERS ----------------------------

:: create output foldwer if it not exist
if not exist "%output%" (
	echo creating %output% folder
    mkdir "%output%"
)

:: remove compile folder (or any other name you set) if it exist.
if exist "%compileOutput%" (
	echo removing %compileOutput%
    rmdir /s /q "%compileOutput%"
)

:: Create new compile folder
echo creating new %compileFolder% folder
mkdir "%compileOutput%"

:: --------------------------- END_CREATE_FOLDERS --------------------------


:: ------------------------------ GET_COMPILE ------------------------------

:: restore packages
echo installing packages
nuget restore "%sln%"

:: getting build
echo getting build in progress...
msbuild "%sln%" /t:Clean,Build /p:Configuration=%buildConfigurationName%;Platform=x86;OutputPath="%compileOutput%"

:: ---------------------------- END_GET_COMPILE ----------------------------


echo Build finished at -------------------- %time%-------------------
pause
exit
