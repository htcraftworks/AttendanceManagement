@ECHO OFF
ECHO ━━━━━━━━━━━━━━━━━━━━━━━
ECHO 管理外のファイルを削除します
ECHO 実行前に作業中のファイルがないか確認をしてください

ECHO ━━━━━━━━━━━━━━━━━━━━━━━

ECHO binフォルダ
ECHO objフォルダ
ECHO distフォルダ
ECHO TestResultsフォルダ
ECHO PublishProfilesフォルダ
ECHO ServiceDependenciesフォルダ
ECHO node_modulesフォルダ
ECHO .vsフォルダ (隠しフォルダ)

set /P InputData="上記削除を実行しますか？(Y/N) "
if "%InputData%" == "Y" (
    goto EXECUTE
) else if "%InputData%" == "y" (
    goto EXECUTE
) else (
    goto END
)

:EXECUTE
for /d /r %%a in ("bin", "obj", "node_modules", "dist", "PublishProfiles", "ServiceDependencies", "TestResults", ".vs") do (
  if exist "%%a" (
    echo Delete: %%a
    if not "%%A" == "attendancemanagement.client\node_modules\vite\bin" (
      rd /s /q "%%a"
    )  
  )
)

:END
echo The end.

pause