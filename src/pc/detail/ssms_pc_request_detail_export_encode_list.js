var PCREQDTLExportEncodeListSDK = window.PCREQDTLExportEncodeListSDK || {};

PCREQDTLExportEncodeListSDK.onAction = async () => {
    Xrm.Utility.showProgressIndicator("Loading")
    try {
        await ShareSdk.addlibAsync()

        const formid = ShareSdk.getCurrentFormId()

        const result = await ShareSdk.getAllPROXCardREQDTLsAsync(formid)

        let sheetContent = {
            "!ref": "A1:C100",
            A1: {
                t: 's', v: "Substation 變電站",

            },
            // B1: { t: 'n', v: 1 },
            B1: { t: 's', v: Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.ss_display).getValue() },
            A2: {
                t: 's', v: "Access From 入站由"
            },
            B2: {
                t: 's', v: Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.accessperiodfrom).getValue().toLocaleDateString([], {
                    year: 'numeric',
                    month: '2-digit',
                    day: '2-digit',
                })
            },
            A3: {
                t: 's', v: "Access To 入站至"
            },
            B3: {
                t: 's', v: Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.accessperiodfrom).getValue().toLocaleDateString([], {
                    year: 'numeric',
                    month: '2-digit',
                    day: '2-digit',
                })
            },

            A4: {
                t: 's', v: 'Card No.',
                s: {
                    fill: { fgColor: { rgb: "ffd700" } },
                    border: {
                        top: { style: 'thin', color: { rgb: "000000" } },
                        // bottom: { style: 'thin', color: { rgb: "000000" } },
                        left: { style: 'thin', color: { rgb: "000000" } },
                        right: { style: 'thin', color: { rgb: "000000" } }
                    }
                }
            },
            B4: {
                t: 's', v: 'Applicant Name', s: {
                    fill: { fgColor: { rgb: "ffd700" } },
                    border: {
                        top: { style: 'thin', color: { rgb: "000000" } },
                        // bottom: { style: 'thin', color: { rgb: "000000" } },
                        left: { style: 'thin', color: { rgb: "000000" } },
                        right: { style: 'thin', color: { rgb: "000000" } }
                    }
                }
            },
            C4: {
                t: 's', v: 'HKID', s: {
                    fill: { fgColor: { rgb: "ffd700" } },
                    border: {
                        top: { style: 'thin', color: { rgb: "000000" } },
                        // bottom: { style: 'thin', color: { rgb: "000000" } },
                        left: { style: 'thin', color: { rgb: "000000" } },
                        right: { style: 'thin', color: { rgb: "000000" } }
                    }
                }
            },


            A5: {
                t: 's', v: '磁咭編號', s: {
                    fill: { fgColor: { rgb: "ffd700" } },
                    border: {
                        // top: { style: 'thin', color: { rgb: "000000" } },
                        bottom: { style: 'thin', color: { rgb: "000000" } },
                        left: { style: 'thin', color: { rgb: "000000" } },
                        right: { style: 'thin', color: { rgb: "000000" } }
                    }
                }
            },
            B5: {
                t: 's', v: '申請人姓名', s: {
                    fill: { fgColor: { rgb: "ffd700" } },
                    border: {
                        // top: { style: 'thin', color: { rgb: "000000" } },
                        bottom: { style: 'thin', color: { rgb: "000000" } },
                        left: { style: 'thin', color: { rgb: "000000" } },
                        right: { style: 'thin', color: { rgb: "000000" } }
                    }
                }
            },
            C5: {
                t: 's', v: '身份證號碼', s: {
                    fill: { fgColor: { rgb: "ffd700" } },
                    border: {
                        // top: { style: 'thin', color: { rgb: "000000" } },
                        bottom: { style: 'thin', color: { rgb: "000000" } },
                        left: { style: 'thin', color: { rgb: "000000" } },
                        right: { style: 'thin', color: { rgb: "000000" } }
                    }
                }
            },

            "!merges": [

            ],
            "!cols": [
                { wpx: 200 },
                { wpx: 100 },
                { wpx: 100 },
                { wpx: 100 },
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
            ]
        }

        PCREQDTLExportEncodeListSDK.appendDetail(sheetContent, result)

        XLSX.writeFile({
            SheetNames: ["Sheet1"],
            Sheets: {
                Sheet1: sheetContent
            }
        }, `PC_ENCODE_EXPORT.xlsx`);



    } catch (err) {
        ShareSdk.formError(err)
    }
    finally {
        Xrm.Utility.closeProgressIndicator()
    }
}

PCREQDTLExportEncodeListSDK.appendDetail = (sheetContent, result) => {
    const startRow = 6
    let index = 0;
    for (var item of result) {
        sheetContent[`A${startRow + index}`] = {
            t: 's', v: item.clp_card_no,
            s: {
                // fill: { fgColor: { rgb: "ffd700" } },
                border: {
                    top: { style: 'thin', color: { rgb: "000000" } },
                    bottom: { style: 'thin', color: { rgb: "000000" } },
                    left: { style: 'thin', color: { rgb: "000000" } },
                    right: { style: 'thin', color: { rgb: "000000" } }
                }
            }
        }
        sheetContent[`B${startRow + index}`] = {
            t: 's', v: item.clp_applicant_name,
            s: {
                // fill: { fgColor: { rgb: "ffd700" } },
                border: {
                    top: { style: 'thin', color: { rgb: "000000" } },
                    bottom: { style: 'thin', color: { rgb: "000000" } },
                    left: { style: 'thin', color: { rgb: "000000" } },
                    right: { style: 'thin', color: { rgb: "000000" } }
                }
            }
        }
        sheetContent[`C${startRow + index}`] = {
            t: 's', v: ``,
            s: {
                // fill: { fgColor: { rgb: "ffd700" } },
                border: {
                    top: { style: 'thin', color: { rgb: "000000" } },
                    bottom: { style: 'thin', color: { rgb: "000000" } },
                    left: { style: 'thin', color: { rgb: "000000" } },
                    right: { style: 'thin', color: { rgb: "000000" } }
                }
            }
        }


        index += 1
    }
}

PCREQDTLExportEncodeListSDK.enableButton = () => {

    var status = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).getValue()
    if (status === ShareSdk.PROXCardReqStatus.WaitingforCardEncoding) {
        return ShareSdk.isProximityCardSecurity()
    }
    return false
}