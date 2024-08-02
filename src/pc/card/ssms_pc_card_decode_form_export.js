var PROXCardDecodeFormExportSDK = window.PROXCardDecodeFormExportSDK || {};

PROXCardDecodeFormExportSDK.onAction = async () => {
    Xrm.Utility.showProgressIndicator("Loading")
    try {
        await ShareSdk.addlibAsync()

        const formid = ShareSdk.getCurrentFormId()
        const applicants = await PROXCardDecodeFormExportSDK.getApplicantAsync(formid)
        const card = await ShareSdk.getPROXCardByIdAsync(formid)

        let retVal = [];
        const list = await PROXCardDecodeFormExportSDK.getEncodedReqeustAsync(formid)
        for (var item of list) {
            const detail = await PROXCardDecodeFormExportSDK.getEncodeDetailAsync(item["request.clp_proximitycardrequestid"])
            const sscodes = await PROXCardDecodeFormExportSDK.getPcEncodedSsByReqAsync(item["request.clp_proximitycardrequestid"], formid)
            let log = {
                "reqeustGuid": item["request.clp_proximitycardrequestid"],
                "appno": detail.clp_name,
                "substation": sscodes,
                "accessfrom": detail["clp_accessperiodfrom@OData.Community.Display.V1.FormattedValue"],
                "accessto": detail["clp_accessperiodto@OData.Community.Display.V1.FormattedValue"],
                "re": detail.clp_ResponsiblePerson.clp_name,
                "deptbranch": detail.clp_re_departmentbranch
            }
            retVal.push(log)
        }

        let sheetContent = {
            "!ref": "A1:D100",
            A1: {
                t: 's', v: "Card No. 磁咭號碼",

            },
            // B1: { t: 'n', v: 1 },
            B1: { t: 's', v: card.clp_name },
            A2: {
                t: 's', v: "Applicant Name 借用人姓名"
            },
            B2: { t: 's', v: applicants.length > 0 ? applicants[0].clp_applicant_name : '' },
            A3: {
                t: 's', v: "Contractor ID 承辦商編號"
            },
            B3: { t: 's', v: applicants.length > 0 ? applicants[0].clp_contractor_id : '' },
            A4: {
                t: 's', v: "Status 狀態",
            },

            B4: {
                t: 's', v: card["clp_card_status@OData.Community.Display.V1.FormattedValue"]
            },
            A5: {
                t: 's', v: 'Applicaiton No.',
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
            B5: {
                t: 's', v: 'Substation', s: {
                    fill: { fgColor: { rgb: "ffd700" } },
                    border: {
                        top: { style: 'thin', color: { rgb: "000000" } },
                        // bottom: { style: 'thin', color: { rgb: "000000" } },
                        left: { style: 'thin', color: { rgb: "000000" } },
                        right: { style: 'thin', color: { rgb: "000000" } }
                    }
                }
            },
            C5: {
                t: 's', v: 'Access From', s: {
                    fill: { fgColor: { rgb: "ffd700" } },
                    border: {
                        top: { style: 'thin', color: { rgb: "000000" } },
                        // bottom: { style: 'thin', color: { rgb: "000000" } },
                        left: { style: 'thin', color: { rgb: "000000" } },
                        right: { style: 'thin', color: { rgb: "000000" } }
                    }
                }
            },
            D5: {
                t: 's', v: 'Access To', s: {
                    fill: { fgColor: { rgb: "ffd700" } },
                    border: {
                        top: { style: 'thin', color: { rgb: "000000" } },
                        // bottom: { style: 'thin', color: { rgb: "000000" } },
                        left: { style: 'thin', color: { rgb: "000000" } },
                        right: { style: 'thin', color: { rgb: "000000" } }
                    }
                }
            },

            A6: {
                t: 's', v: '申請編號', s: {
                    fill: { fgColor: { rgb: "ffd700" } },
                    border: {
                        // top: { style: 'thin', color: { rgb: "000000" } },
                        bottom: { style: 'thin', color: { rgb: "000000" } },
                        left: { style: 'thin', color: { rgb: "000000" } },
                        right: { style: 'thin', color: { rgb: "000000" } }
                    }
                }
            },
            B6: {
                t: 's', v: '變電站', s: {
                    fill: { fgColor: { rgb: "ffd700" } },
                    border: {
                        // top: { style: 'thin', color: { rgb: "000000" } },
                        bottom: { style: 'thin', color: { rgb: "000000" } },
                        left: { style: 'thin', color: { rgb: "000000" } },
                        right: { style: 'thin', color: { rgb: "000000" } }
                    }
                }
            },
            C6: {
                t: 's', v: '入站由', s: {
                    fill: { fgColor: { rgb: "ffd700" } },
                    border: {
                        // top: { style: 'thin', color: { rgb: "000000" } },
                        bottom: { style: 'thin', color: { rgb: "000000" } },
                        left: { style: 'thin', color: { rgb: "000000" } },
                        right: { style: 'thin', color: { rgb: "000000" } }
                    }
                }
            },
            D6: {
                t: 's', v: '入站至', s: {
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

        PROXCardDecodeFormExportSDK.appendDetail(sheetContent, retVal)

        XLSX.writeFile({
            SheetNames: ["Sheet1"],
            Sheets: {
                Sheet1: sheetContent
            }
        }, `PC_DECODE_CARD_EXPORT.xlsx`);



    } catch (err) {
        ShareSdk.formError(err)
    }
    finally {
        Xrm.Utility.closeProgressIndicator()
    }
}

PROXCardDecodeFormExportSDK.appendDetail = (sheetContent, details) => {
    const startRow = 7
    let index = 0;
    for (var item of details) {
        sheetContent[`A${startRow + index}`] = {
            t: 's', v: item.appno,
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
            t: 's', v: item.substation,
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
            t: 's', v: item.accessfrom,
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
        sheetContent[`D${startRow + index}`] = {
            t: 's', v: item.accessto,
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

PROXCardDecodeFormExportSDK.getApplicantAsync = (cardid) => {
    const fetchXml = `<fetch>
  <entity name="clp_proximity_card_request_detail">
    <attribute name="clp_applicant_name" />
    <attribute name="clp_contractor_id" />
    <filter>
      <condition attribute="clp_proximity_card" operator="eq" value="${cardid}" />
      <condition attribute="clp_detail_status" operator="eq" value="768230003" />
    </filter>
    <link-entity name="clp_proximitycardrequest" from="clp_proximitycardrequestid" to="clp_proximity_card_request">
      <filter>
        <condition attribute="clp_workflow_status" operator="eq" value="768230005" />
      </filter>
    </link-entity>
  </entity>
</fetch>`
    return fetch(
        `/api/data/v9.0/clp_proximity_card_request_details?fetchXml=${encodeURI(fetchXml)}`,
        {
            headers: {
                "Content-Type": "application/json",
                'Prefer': 'odata.include-annotations="*"',
            }
        }
    ).then(res => res.json()).then(res => {
        if (res.error) {
            throw new Error(res.error.message)
        }
        return Promise.resolve(res.value)
    }).catch(err => {
        console.error(err)
        throw err;
    })
}

PROXCardDecodeFormExportSDK.getEncodedReqeustAsync = (cardid) => {
    const fetchXml = `<fetch distinct="true">
  <entity name="clp_proximity_card_encode">
    <link-entity name="clp_proximitycardinventory" from="clp_proximitycardinventoryid" to="clp_card">
      <filter>
        <condition attribute="clp_proximitycardinventoryid" operator="eq" value="${cardid}" />
      </filter>
    </link-entity>
    <link-entity name="clp_proximitycardrequest" from="clp_proximitycardrequestid" to="clp_proximity_card_request" alias="request">
      <attribute name="clp_proximitycardrequestid" />
    </link-entity>
  </entity>
</fetch>`
    return fetch(
        `/api/data/v9.0/clp_proximity_card_encodes?fetchXml=${encodeURI(fetchXml)}`,
        {
            headers: {
                "Content-Type": "application/json",
                'Prefer': 'odata.include-annotations="*"',
            }
        }
    ).then(res => res.json()).then(res => {
        if (res.error) {
            throw new Error(res.error.message)
        }
        return Promise.resolve(res.value)
    }).catch(err => {
        console.error(err)
        throw err;
    })
}

PROXCardDecodeFormExportSDK.getEncodeDetailAsync = (reqeustGuid) => {
    return fetch(
        `/api/data/v9.0/clp_proximitycardrequests(${reqeustGuid})?$expand=clp_ResponsiblePerson($select=clp_name,clp_dept_name,clp_branch_name)`,
        {
            headers: {
                "Content-Type": "application/json",
                'Prefer': 'odata.include-annotations="*"',
            }
        }
    ).then(res => res.json()).then(res => {
        if (res.error) {
            throw new Error(res.error.message)
        }
        return Promise.resolve(res)
    }).catch(err => {
        console.error(err)
        throw err;
    })
}

PROXCardDecodeFormExportSDK.getPcEncodedSsByReqAsync = async (requestGuid, cardid) => {
    const list = await ShareSdk.getPROXCardEncodesByCardandReqAsync(cardid, requestGuid)
    return list.map(p => p.clp_ss_code).join()
}


PROXCardDecodeFormExportSDK.enableButton = () => {
    return ShareSdk.isProximityCardSecurity() && ShareSdk.IsProxCardDecodeForm()
}