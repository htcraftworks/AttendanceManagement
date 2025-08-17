/** services */
import apisService from "../../../services/api/apisService";

/** services */
import localStorageService from "../../../services/storage/localStorageService";

/** types */
import { type UserModel } from "../../../types/UserModel";
import { type UserCreateParam } from "../../../services/api/types/param/UserCreateParam";
import { type UILogicResultModel } from "../../App/types/UILogicResultModel";
import { type LoginParam } from "../../../services/api/types/param/LoginParam";
import { type UserResult } from "../../../services/api/types/result/UserResult";

/** class */
import LogicBase from "../../App/logic/logicBase";

/** 
 * アカウントロジック 
 * NOTE:メッセージリソースがhookを使用してるので動的クラスとする
 * */
export default class AccountLogic extends LogicBase {

    /**
     * 有効ユーザ確認
     * @returns 結果
     */
    async CurrentUser(): Promise<UILogicResultModel<UserModel>> {

        let isSuccess: boolean = false;
        let userModel: UserModel | null = null;

        /** 有効ユーザ確認 */
        await apisService.Accounts.currentUser().then(result => {
            const resultData = result.data;
            if (resultData
                && resultData.value) {
                isSuccess = true;

                userModel = {
                    userName: resultData.value.userName
                } as UserModel;;
            }
        }).catch(error => {
            console.log(error);
        });

        return {
            isSuccess: isSuccess,
            messageTitle: null,
            message: null,
            value: userModel
        } as UILogicResultModel<UserModel>;
    }

    /**
      * ログイン
      * @param loginParam ユーザー情報
      * @returns 結果
      */
    async Login(loginParam: LoginParam): Promise<UILogicResultModel<UserModel>> {

        let isSuccess: boolean = false;
        let messageTitle: string = "";
        let message: string = "";

        let userModel: UserModel | null = null;
        let textError = this._msg.getMsg("TextError");

        /** ログイン */
        await apisService.Accounts.login(loginParam).then(result => {
            const resultData = result.data;
            if (resultData) {
                if (resultData.isSuccess) {
                    isSuccess = true;

                    userModel = {
                        userName: resultData.value.userName
                    } as UserModel;

                } else {
                    messageTitle = `${textError}[ErrorCode:${resultData.errorCode}]`;
                    message = `${resultData.errorMessage}`;
                }
            } else {
                messageTitle = textError;
                message = this._msg.getMsg("TextErrorFatal");
            }
        }).catch(error => {
            const [tmpmessageTitle, tmpMessagge] = this.executeApiError<UserResult>(error);
            messageTitle = tmpmessageTitle;
            message = tmpMessagge;
        });

        return {
            isSuccess: isSuccess,
            messageTitle: messageTitle,
            message: message,
            value: userModel
        } as UILogicResultModel<UserModel>;
    }

    /**
     * ログアウト
     * @param loginParam ユーザー情報
     * @returns 結果
     */
    async Logout(): Promise<UILogicResultModel<null>> {

        /** ログアウト */
        await apisService.Accounts.logout().catch(error => {
            console.log(error);
        }).finally(() => {
            localStorageService.removeAuthToken();
        });

        return {
            isSuccess: true,
            messageTitle: null,
            message: null
        } as UILogicResultModel<null>;
    }

    /**
     * ユーザー作成
     * @param loginParam ユーザー情報
     * @returns 結果
     */
    async CreateUser(loginParam: UserCreateParam): Promise<UILogicResultModel<null>> {

        let textError = this._msg.getMsg("TextError");
        let isSuccess: boolean = false;
        let messageTitle: string = "";
        let message: string = "";

        /** ユーザ作成 */
        await apisService.Accounts.userCreate(loginParam).then(result => {
            const resultData = result.data;
            if (resultData) {
                if (resultData.isSuccess) {
                    isSuccess = true;

                    messageTitle = this._msg.getMsg("TextInfo");
                    message = this._msg.getMsg("MessageAccountCreate");
                } else {
                    messageTitle = textError;
                    message = resultData.errorMessage;
                }
            } else {
                messageTitle = textError;
                message = this._msg.getMsg("TextErrorFatal");
            }
        }).catch(error => {
            const [tmpmessageTitle, tmpMessagge] = this.executeApiError<boolean>(error);
            messageTitle = tmpmessageTitle;
            message = tmpMessagge;
        });

        return {
            isSuccess: isSuccess,
            messageTitle: messageTitle,
            message: message
        } as UILogicResultModel<null>;
    }
}