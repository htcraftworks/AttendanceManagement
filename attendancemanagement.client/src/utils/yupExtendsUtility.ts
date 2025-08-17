/** npm */
import * as yup from "yup";

/** types */
import { type YupMessageModel } from "./types/YupMessageModel"
import { type YupStringValidationModel } from "./types/YupStringValidationModel"

/**
 * 検証ラベル名有無
 * @param prm メッセージ
 * @returns ラベル名
 */
const labelText = (prm: YupMessageModel) => {
    return prm.label !== "" ? `${prm.label}は` : ""
}

/** 
 * ロケール毎のメッセージ
 * TODO:多言語化
 *  */

const jpConfig = {
    string: {
        length: (prm: YupMessageModel & { length: number }) =>
            `${labelText(prm)}${prm.length}文字以下で入力してください`,
        min: (prm: YupMessageModel & { min: number }) =>
            `${labelText(prm)}少なくとも${prm.min}文字以上で入力してください`,
        max: (prm: YupMessageModel & { max: number }) =>
            `${labelText(prm)}最大${prm.max}文字以下で入力してください`,
        hanAlphaNumber: (prm: YupMessageModel) => `${labelText(prm)}半角英数字で入力してください`,
    },
}

/** ロケール */
yup.setLocale(jpConfig)

/** 検証情報配列 */
const stringValidationList: YupStringValidationModel[] = [
    {
        name: "hanAlphaNumber",
        errorMessage: jpConfig.string.hanAlphaNumber,
        isValid: (value: string) => {
            return /^[0-9a-zA-Z]*$/.test(value)
        },
    },
]

/** 検証情報による検証 */
stringValidationList.forEach((validation) => {
    yup.addMethod<yup.StringSchema>(
        yup.string,
        validation.name,
        function (message: yup.Message = validation.errorMessage) {
            return this.test(function (value, testContext) {
                if (value == null || value === "") {
                    return true
                }
                if (validation.isValid(value)) {
                    return true
                }
                return testContext.createError({
                    message,
                })
            })
        },
    )
})

/** yupライブラリ拡張 */
declare module "yup" {

    /** スキーマ追加 */
    interface StringSchema<TType, TContext, TDefault, TFlags> {

        /** 半角英数字入力 */
        hanAlphaNumber(): StringSchema<TType, TContext, TDefault, TFlags>
    }
}

/** エクスポート */
export default yup