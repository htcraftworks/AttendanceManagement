@ECHO OFF
ECHO ����������������������������������������������
ECHO �Ǘ��O�̃t�@�C�����폜���܂�
ECHO ���s�O�ɍ�ƒ��̃t�@�C�����Ȃ����m�F�����Ă�������

ECHO ����������������������������������������������

ECHO bin�t�H���_
ECHO obj�t�H���_
ECHO dist�t�H���_
ECHO TestResults�t�H���_
ECHO PublishProfiles�t�H���_
ECHO ServiceDependencies�t�H���_
ECHO node_modules�t�H���_
ECHO .vs�t�H���_ (�B���t�H���_)

set /P InputData="��L�폜�����s���܂����H(Y/N) "
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