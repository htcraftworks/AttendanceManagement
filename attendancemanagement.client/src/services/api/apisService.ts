/** npm */
import axios, { type AxiosResponse, AxiosHeaders } from "axios";
import localStorageService from "../../services/storage/localStorageService";

/** consts */
import ApiRouteAccountConsts from "../../consts/apiRouteAccountConsts";
import ApiRouteAttendanceConsts from "../../consts/apiRouteAttendanceConsts";
import CustomHeaderPrefixConsts from "../../consts/customHeaderPrefixConsts";

/** types */
import { type ApiResultModel } from "./types/ApiResultModel";
import { type ServiceResult } from "./types/result/ServiceResult";
import { type UserResult } from "./types/result/UserResult";
import { type AttendanceRecordsResult } from "./types/result/AttendanceRecordsResult";
import { type LoginParam } from "./types/param/LoginParam";
import { type UserCreateParam } from "./types/param/UserCreateParam";
import { type AttendanceUpdateParam } from "./types/param/AttendanceUpdateParam";
import { type AttendanceGetParam } from "./types/param/AttendanceGetParam";
import { type AttendanceGetForMonthParam } from "./types/param/AttendanceGetForMonthParam";
import { type AttendanceDeleteParam } from "./types/param/AttendanceDeleteParam";
import { type AttendanceClockInOutParam } from "./types/param/AttendanceClockInOutParam";

// ベースURL
axios.defaults.baseURL = import.meta.env.VITE_APP_BASE_URL;

// 共通レスポンス処理関数
function unwrapResponse<T>(response: AxiosResponse<ServiceResult<T>>): ApiResultModel<T> {
    return {
        statusCode: response.status,
        data: response.data,
    };
}

// 毎回認証トークン情報を含める
axios.interceptors.request.use(config => {
    const token = localStorageService.getAuthToken();

    const headers = config.headers instanceof AxiosHeaders
        ? config.headers
        : new AxiosHeaders(config.headers);

    if (token) {
        headers.set("Authorization", CustomHeaderPrefixConsts.AuthToken.prefix + token);
    }

    config.headers = headers;
    return config;
});

// サーバーから返ってくるJWTトークンをレスポンスヘッダーから取得して保存
axios.interceptors.response.use(response => {
    // Authorizationヘッダーは小文字で来ることもあるので両方チェック
    const headerLower = CustomHeaderPrefixConsts.AuthToken.headerNameLowercase;
    const headerOriginal = CustomHeaderPrefixConsts.AuthToken.headerName;
    const newToken = response.headers[headerLower] || response.headers[headerOriginal];

    if (newToken) {
		// プレフィックス「"Bearer "」を除外
        const prefix = CustomHeaderPrefixConsts.AuthToken.prefix;
        const token = newToken.startsWith(prefix) ? newToken.substring(prefix.length) : newToken;
        localStorageService.setAuthToken(token);
    }

    return response;
}, error => {
    return Promise.reject(error);
});

// アカウント関連のAPI
const Accounts = {
    currentUser: (): Promise<ApiResultModel<UserResult>> =>
        axios.get<ServiceResult<UserResult>>(ApiRouteAccountConsts.UserCurrent)
            .then(unwrapResponse),

    login: (loginParam: LoginParam): Promise<ApiResultModel<UserResult>> =>
        axios.post<ServiceResult<UserResult>>(ApiRouteAccountConsts.Login, loginParam)
            .then(unwrapResponse),

    logout: (): Promise<ApiResultModel<UserResult>> =>
        axios.get<ServiceResult<UserResult>>(ApiRouteAccountConsts.Logout)
            .then(unwrapResponse),

    userCreate: (accountParam: UserCreateParam): Promise<ApiResultModel<boolean>> =>
        axios.post<ServiceResult<boolean>>(ApiRouteAccountConsts.UserCreate, accountParam)
            .then(unwrapResponse),
};

/** 勤怠関連のAPI */
const Attendances = {
    attendanceClockInState: (): Promise<ApiResultModel<boolean>> =>
        axios.get<ServiceResult<boolean>>(ApiRouteAttendanceConsts.AttendanceClockInState)
            .then(unwrapResponse),

    attendanceClockIn: (attendanceClockInOutParam: AttendanceClockInOutParam): Promise<ApiResultModel<boolean>> =>
        axios.post<ServiceResult<boolean>>(ApiRouteAttendanceConsts.AttendanceClockIn, attendanceClockInOutParam)
            .then(unwrapResponse),

    attendanceClockOut: (attendanceClockInOutParam: AttendanceClockInOutParam): Promise<ApiResultModel<boolean>> =>
        axios.post<ServiceResult<boolean>>(ApiRouteAttendanceConsts.AttendanceClockOut, attendanceClockInOutParam)
            .then(unwrapResponse),

    attendanceGet: (attendanceGetParam: AttendanceGetParam): Promise<ApiResultModel<AttendanceRecordsResult>> =>
        axios.post<ServiceResult<AttendanceRecordsResult>>(ApiRouteAttendanceConsts.AttendanceGet, attendanceGetParam)
            .then(unwrapResponse),

    attendanceGetForMonth: (attendanceGetForMonthParam: AttendanceGetForMonthParam): Promise<ApiResultModel<AttendanceRecordsResult[]>> =>
        axios.post<ServiceResult<AttendanceRecordsResult[]>>(ApiRouteAttendanceConsts.AttendanceGetForMonth, attendanceGetForMonthParam)
            .then(unwrapResponse),

    attendanceUpdate: (attendanceUpdateParam: AttendanceUpdateParam): Promise<ApiResultModel<boolean>> =>
        axios.post<ServiceResult<boolean>>(ApiRouteAttendanceConsts.AttendanceUpdate, attendanceUpdateParam)
            .then(unwrapResponse),

    attendanceDelete: (attendanceDeleteParam: AttendanceDeleteParam): Promise<ApiResultModel<boolean>> =>
        axios.post<ServiceResult<boolean>>(ApiRouteAttendanceConsts.AttendanceDelete, attendanceDeleteParam)
            .then(unwrapResponse),
};

/** APIグループ */
const Actions = {
    Accounts,
    Attendances,
};

export default Actions;
