var PROXCardReqExportUtilitiesSDK = window.PROXCardReqExportUtilitiesSDK || {};
window.parent.PROXCardReqExportUtilitiesSDK = window.PROXCardReqExportUtilitiesSDK || {};


PROXCardReqExportUtilitiesSDK.export = async () => {

    Xrm.Utility.showProgressIndicator("Loading")
    try {
        await ShareSdk.addlibAsync()

        const formid = ShareSdk.getCurrentFormId()

        await PROXCardReqExportUtilitiesSDK.exportFile(formid)

    } catch (err) {
        ShareSdk.formError(err)
    }
    finally {
        Xrm.Utility.closeProgressIndicator()
    }
}

PROXCardReqExportUtilitiesSDK.exportFile = async (requestid) => {

    const request = await ShareSdk.getPROXCardRequestAsync(requestid)


    let sheetContent = {
        "!ref": "A1:AR100",
        E2: {
            t: 's', v: "進入中電輸電變電站\n新電腦磁咭 / 現有磁咭編碼 申請表",
            s: {
                alignment: { wrapText: true, vertical: "center", horizontal: "center" },
                font: { name: "細明體", sz: 12, bold: true, }
            }

        },
        // B1: { t: 'n', v: 1 },
        B5: { t: 's', v: '申請編號:' },
        D5: {
            t: 's', v: request.clp_name, s: {
                font: { name: "ARIAL", sz: 10 }
            }
        },
        K5: { t: 's', v: '申請日期:' },
        P5: {
            t: 's', v: request["clp_requestdate@OData.Community.Display.V1.FormattedValue"],
            s: {
                font: { name: "ARIAL", sz: 10 }
            }
        },
        X5: { t: 's', v: '狀態:' },
        AA5: {
            t: 's', v: request['clp_workflow_status@OData.Community.Display.V1.FormattedValue'],
            s: {
                font: { name: "ARIAL", sz: 10 }
            }
        },

        B7: {
            t: 's', v: '申請人填寫',
            s: {
                font: { name: "細明體", sz: 10, bold: true, underline: true }
            }
        },
        B9: { t: 's', v: '承包商/訪客商號:' },
        G9: { t: 's', v: request["_clp_lut_ctrcomp_value@OData.Community.Display.V1.FormattedValue"] },
        X9: { t: 's', v: '中電工作指示編號:' },
        AF9: { t: 's', v: request.clp_clpworkorderno || '' },

        B11: { t: 's', v: '要進入的變電站:' },
        G11: { t: 's', v: request.clp_ss_display },
        G13: { t: 's', v: `(WE:${request.clp_ss_we} NR:${request.clp_ss_nr})` },

        B15: { t: 's', v: '工作內容:' },
        G15: { t: 's', v: request.clp_workcontent },

        B17: { t: 's', v: '入站工作日期:' },
        G17: { t: 's', v: '由' },
        I17: { t: 's', v: request["clp_accessperiodfrom@OData.Community.Display.V1.FormattedValue"] },
        M17: { t: 's', v: '至' },
        O17: { t: 's', v: request["clp_accessperiodto@OData.Community.Display.V1.FormattedValue"] },
        R17: { t: 's', v: '(ISMS 磁咭不可超過六個月)' },

        B19: { t: 's', v: '負責幹練人員姓名:' },
        G19: { t: 's', v: request["_clp_responsible_cp_value@OData.Community.Display.V1.FormattedValue"] ?? request.clp_responsiblecp },
        S19: { t: 's', v: '授權證書號碼:' },
        X19: { t: 's', v: request.clp_authorizationcode },
        AD19: { t: 's', v: '聯絡電話:' },
        AG19: { t: 's', v: request.clp_phoneno },

        B21: { t: 's', v: '承包商負責人姓名:' },
        G21: { t: 's', v: request.clp_contractorresponsibleperson },
        AD21: { t: 's', v: '聯絡電話:' },
        AG21: { t: 's', v: request.clp_contractorphoneno },

        B23: {
            t: 's', v: '所有申請進入中電輸電變電站工作人員必須獨立填寫個人資料如下:',
            s: {
                font: { name: "細明體", sz: 10, bold: true }
            }
        },

        C26: {
            t: 's', v: '類別',
            s: {
                font: { name: "細明體", sz: 10, underline: true, }
            }
        },
        H26: {
            t: 's', v: '磁咭號碼',
            s: {
                font: { name: "細明體", sz: 10, underline: true, }
            }
        },
        L26: {
            t: 's', v: '申請人姓名',
            s: {
                font: { name: "細明體", sz: 10, underline: true, }
            }
        },
        U25: {
            t: 's', v: '承辦商\n編號',
            s: {
                alignment: { wrapText: true, vertical: "center", horizontal: "center" },
                font: { name: "細明體", sz: 10, underline: true, }
            }
        },
        Z26: {
            t: 's', v: '聯絡電話',
            s: {
                font: { name: "細明體", sz: 10, underline: true, }
            }
        },
        AF26: {
            t: 's', v: '狀態',
            s: {
                font: { name: "細明體", sz: 10, underline: true, }
            }
        },
        AK26: {
            t: 's', v: '備註/拒絕原因',
            s: {
                font: { name: "細明體", sz: 10, underline: true, }
            }
        },

        "!merges": [
            { s: { r: 1, c: 4 }, e: { r: 2, c: 25 + 6 } }, /*E2 AG3*/ //title
            { s: { r: 4, c: 1 }, e: { r: 4, c: 2 } }, /*B5 C5*/ //申請編號
            { s: { r: 4, c: 3 }, e: { r: 4, c: 8 } }, // applition no
            { s: { r: 4, c: 10 }, e: { r: 4, c: 14 } },  // request date text
            { s: { r: 4, c: 15 }, e: { r: 4, c: 18 } },  // reqeust value
            { s: { r: 4, c: 23 }, e: { r: 4, c: 25 } },  //status text
            { s: { r: 4, c: 26 }, e: { r: 4, c: 32 } },  //format status


            { s: { r: 6, c: 1 }, e: { r: 6, c: 5 } }, //申請人填寫
            { s: { r: 8, c: 1 }, e: { r: 8, c: 5 } }, //承包商/訪客商號:
            { s: { r: 8, c: 6 }, e: { r: 8, c: 20 } },
            { s: { r: 8, c: 23 }, e: { r: 8, c: 29 } },//中電工作指示編號

            { s: { r: 10, c: 1 }, e: { r: 10, c: 5 } },
            { s: { r: 10, c: 6 }, e: { r: 10, c: 42 } },
            { s: { r: 12, c: 6 }, e: { r: 12, c: 20 } },

            { s: { r: 14, c: 1 }, e: { r: 14, c: 5 } },
            { s: { r: 14, c: 6 }, e: { r: 14, c: 42 } },

            { s: { r: 16, c: 1 }, e: { r: 16, c: 5 } },
            { s: { r: 16, c: 6 }, e: { r: 16, c: 7 } },
            { s: { r: 16, c: 8 }, e: { r: 16, c: 11 } },
            { s: { r: 16, c: 12 }, e: { r: 16, c: 13 } },
            { s: { r: 16, c: 14 }, e: { r: 16, c: 16 } },
            { s: { r: 16, c: 17 }, e: { r: 16, c: 26 } },

            { s: { r: 18, c: 1 }, e: { r: 18, c: 5 } },
            { s: { r: 18, c: 6 }, e: { r: 18, c: 15 } },
            { s: { r: 18, c: 18 }, e: { r: 18, c: 22 } },
            { s: { r: 18, c: 23 }, e: { r: 18, c: 27 } },
            { s: { r: 18, c: 26 + 3 }, e: { r: 18, c: 26 + 3 + 2 } },  // PHONE
            { s: { r: 18, c: 26 + 3 + 2 + 1 }, e: { r: 18, c: 26 + 3 + 2 + 1 + 3 } },

            { s: { r: 20, c: 1 }, e: { r: 20, c: 5 } },
            { s: { r: 20, c: 6 }, e: { r: 20, c: 26 } },
            { s: { r: 20, c: 26 + 3 }, e: { r: 20, c: 26 + 3 + 2 } },  // PHONE
            { s: { r: 20, c: 26 + 3 + 2 + 1 }, e: { r: 20, c: 26 + 3 + 2 + 1 + 3 } },

            { s: { r: 22, c: 1 }, e: { r: 22, c: 27 } },

            { s: { r: 25, c: 2 }, e: { r: 25, c: 6 } },
            { s: { r: 25, c: 7 }, e: { r: 25, c: 10 } },
            { s: { r: 25, c: 11 }, e: { r: 25, c: 19 } },
            { s: { r: 24, c: 20 }, e: { r: 25, c: 23 } },
            { s: { r: 25, c: 25 }, e: { r: 25, c: 30 } },
            { s: { r: 25, c: 31 }, e: { r: 25, c: 34 } },
            { s: { r: 25, c: 36 }, e: { r: 25, c: 37 } },
        ],
        "!cols": [
            { wpx: 20 },
            { wpx: 20 },
            { wpx: 40 },
            { wpx: 20 },
            { wpx: 20 },
            { wpx: 20 },
            { wpx: 20 },
            { wpx: 20 },
            { wpx: 20 },
            { wpx: 20 },
            { wpx: 20 },
            { wpx: 20 },
            { wpx: 20 },
            { wpx: 20 },
            { wpx: 20 },
            { wpx: 20 },
            { wpx: 20 },
            { wpx: 20 },
            { wpx: 20 },
            { wpx: 20 },
            { wpx: 20 },
            { wpx: 20 },
            { wpx: 20 },
            { wpx: 20 },
            { wpx: 20 },
            { wpx: 20 },
            { wpx: 20 },
            { wpx: 20 },
            { wpx: 20 },
            { wpx: 20 },
            { wpx: 20 },
            { wpx: 20 },
            { wpx: 20 },
            { wpx: 20 },
            { wpx: 20 },
            { wpx: 20 },
            { wpx: 20 },
            { wpx: 20 },//AL
        ]
    }

    let startRowIndex = appendDetail(sheetContent, request)
    startRowIndex = appendFooter(sheetContent, request, startRowIndex + 3)
    startRowIndex = appendFooter(sheetContent, request, startRowIndex + 2)

    await appendRemarks(sheetContent, request, startRowIndex + 2)

    XLSX.writeFile({
        SheetNames: ["Sheet1"],
        Sheets: {
            Sheet1: sheetContent
        }
    }, `${request.clp_name}.xlsx`);

}

const appendDetail = (sheetContent, request) => {
    const startRow = 28
    let index = 0;
    let i = 1
    for (var item of request.clp_pc_request_detail_pc_request) {
        //index
        sheetContent[`B${startRow + index}`] = { t: 's', v: `${i}` }
        //类别
        sheetContent[`C${startRow + index}`] = { t: 's', v: item["clp_app_type@OData.Community.Display.V1.FormattedValue"] }
        sheetContent[`H${startRow + index}`] = { t: 's', v: item.clp_card_no || '' }
        sheetContent[`L${startRow + index}`] = { t: 's', v: item.clp_applicant_name || '' }
        //承辦商編號		
        //phone	
        sheetContent[`Z${startRow + index}`] = { t: 's', v: item.clp_phone_no || '' }
        //状态
        sheetContent[`AF${startRow + index}`] = { t: 's', v: item["clp_detail_status@OData.Community.Display.V1.FormattedValue"] || '' }
        //remark
        sheetContent[`AK${startRow + index}`] = { t: 's', v: item.clp_remarks || '' }
        index += 1
        i++
    }

    return startRow + index
}

const appendFooter = (sheetContent, request, startRowIndex) => {
    sheetContent[`B${startRowIndex}`] = {
        t: 's', v: '負責工程師填寫',
        s: {
            font: { name: "細明體", sz: 10, underline: true, bold: true }
        }
    }

    sheetContent[`B${startRowIndex + 2}`] = { t: 's', v: '發咭部門:' }
    sheetContent[`F${startRowIndex + 2}`] = { t: 's', v: request["clp_issuedepartment@OData.Community.Display.V1.FormattedValue"] }

    sheetContent[`B${startRowIndex + 4}`] = { t: 's', v: '已選批准人:' }
    sheetContent[`F${startRowIndex + 4}`] = { t: 's', v: request["_clp_chosenapprover_value@OData.Community.Display.V1.FormattedValue"] }
    sheetContent[`S${startRowIndex + 4}`] = { t: 's', v: '部門/支部:' }
    sheetContent[`W${startRowIndex + 4}`] = { t: 's', v: request.clp_approver_deptbranch }

    return startRowIndex + 4
}

const appendRemarks = async (sheetContent, request, remarkStartRowIndex) => {

    let retVal = [];
    let log = await addRe(request, '負責工程師:')
    if (log) {
        retVal.push(log)
    }

    log = await addApprover(request, '批准人:')
    if (log) {
        retVal.push(log)
    }

    log = await addReview(request)
    if (log) {
        retVal.push(log)
    }

    log = await addmm(request)
    if (log) {
        retVal.push(log)
    }

    log = await addAdmin(request)
    if (log) {
        retVal.push(log)
    }

    log = await addSecurity(request)
    if (log) {
        retVal.push(log)
    }


    sheetContent[`B${remarkStartRowIndex}`] = {
        t: 's', v: '備註',
        s: {
            font: { name: "細明體", sz: 10, underline: true, bold: true }
        }
    }
    let startRowIndex = remarkStartRowIndex + 2
    for (var item of retVal) {

        sheetContent[`B${startRowIndex}`] = { t: 's', v: item.role }
        sheetContent[`F${startRowIndex}`] = { t: 's', v: item.name }
        sheetContent[`S${startRowIndex}`] = { t: 's', v: '部門/支部:' }
        sheetContent[`W${startRowIndex}`] = { t: 's', v: item.deptbranch }
        sheetContent[`AK${startRowIndex}`] = { t: 's', v: '日期:' }
        sheetContent[`AL${startRowIndex}`] = { t: 's', v: item.date }
        sheetContent[`B${startRowIndex + 1}`] = { t: 's', v: '備註:' }
        sheetContent[`F${startRowIndex + 1}`] = { t: 's', v: item.remarks }

        startRowIndex += 3
    }

}

const addRe = (request, role) => {
    return addRemarks(role, request["_clp_responsibleperson_value@OData.Community.Display.V1.FormattedValue"],
        request._clp_responsibleperson_value,
        request["clp_requestdate@OData.Community.Display.V1.FormattedValue"],
        request.clp_remarks || ''
    )
}

const addApprover = (request, role) => {
    return addRemarks(role, request["_clp_approver_value@OData.Community.Display.V1.FormattedValue"],
        request._clp_approver_value,
        request["clp_appr_date@OData.Community.Display.V1.FormattedValue"],
        request.clp_appr_remarks || ''
    )
}
const addReview = (request) => {
    return addRemarks('覆核人',
        request["_clp_reviewer_value@OData.Community.Display.V1.FormattedValue"],
        request._clp_reviewer_value,
        request["clp_reviewed_date@OData.Community.Display.V1.FormattedValue"],
        request.clp_reviewer_remarks
    )
}

const addmm = (request) => {
    return addRemarks('首席土木經理',
        request["_clp_mm_value@OData.Community.Display.V1.FormattedValue"],
        request._clp_mm_value,
        request["clp_mm_date@OData.Community.Display.V1.FormattedValue"],
        request.clp_mm_remarks
    )
}

const addAdmin = (request) => {
    return addRemarks('行政處',
        request["_clp_admin_value@OData.Community.Display.V1.FormattedValue"],
        request._clp_admin_value,
        request["clp_card_allocation_date@OData.Community.Display.V1.FormattedValue"],
        request.clp_admin_remarks
    )
}

const addSecurity = (request) => {
    return addRemarks('保安部',
        request["_clp_security_value@OData.Community.Display.V1.FormattedValue"],
        request._clp_security_value,
        request["clp_card_encoding_date@OData.Community.Display.V1.FormattedValue"],
        request.clp_security_remarks
    )
}


const addRemarks = async (role, approver, userid, date, remarks) => {
    if (userid) {

        var deptbranch = await ShareSdk.getDeptBranchAsync(userid)
        let log = { "role": role, "name": approver, "deptbranch": deptbranch || '', "date": date || '--', "remarks": remarks || '' };
        return log
    }
    return null

}


PROXCardReqExportUtilitiesSDK.addlibAsync = async () => {
    var jscontent = await fetch('/WebResources/clp_xlsx.core.min.js', { method: 'GET' })
        .then(r => r.text());
    window.eval(jscontent);
    jscontent = await fetch('/WebResources/clp_xlsx.bundle.js', { method: 'GET' })
        .then(r => r.text());
    window.eval(jscontent);
}