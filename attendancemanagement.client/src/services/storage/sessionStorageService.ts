/** consts */
import SessionStorageKeyConsts from "../../consts/sessionStorageKeyConsts"

/** セッションストレージ操作 */
export default class sessionStorageService {

    /**
     * ユーザー名を取得します
     */
    static getUserName(): string | null {
        return sessionStorageService.getSessionStorage(SessionStorageKeyConsts.USER_NAME);
    }

    /**
     * ユーザー名を登録します
     * @param value 値
     */
    static setUserName(value: string): void {
        sessionStorageService.setSessionStorage(SessionStorageKeyConsts.USER_NAME, value);
    }

    /**
     * ユーザー名を削除します
     */
    static removeUserName(): void {
        sessionStorageService.removeSessionStorage(SessionStorageKeyConsts.USER_NAME);
    }

    /**
     * SessionStorageの対象データを取得します
     * @param key キー
     */
    private static getSessionStorage(key: string): string | null {
        return sessionStorage.getItem(key);
    }

    /**
     * SessionStorageにデータを登録します
     * @param key キー
     * @param value 値
     */
    private static setSessionStorage(key: string, value: string): void {
        sessionStorage.setItem(key, value);
    }

    /**
     * SessionStorageの対象データを削除します
     * @param key キー
     */
    private static removeSessionStorage(key: string): void {
        sessionStorage.removeItem(key);
    }
}