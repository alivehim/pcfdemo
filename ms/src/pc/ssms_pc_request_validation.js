var PROXCardReqValidatinoSDK = window.PROXCardReqValidatinoSDK || {};
window.parent.PROXCardReqValidatinoSDK = window.PROXCardReqValidatinoSDK || {};

PROXCardReqValidatinoSDK.submissionValidateAsync = async () => {


    // applicants in one requisition form can not exceed 20
    var formid = ShareSdk.getCurrentFormId()

    PROXCardReqValidatinoSDK.checkSubstationsIsFilled()

    if (!(await PROXCardReqValidatinoSDK.checkIfAllC_CardAsync())) {

        PROXCardReqValidatinoSDK.validateAccessPeriod()
    }


    var applicants = await ShareSdk.getPROXCardRequestDetailsAsync(formid)
    if (applicants.length > 20) {
        throw new Error("At most 20 applications of new ISMS Proximity Card, encode ISMS Proximity Card and encode Contractor/Consultant Pass can be included in one Requisition Form")
    }
    else if (applicants.length === 0) {
        throw new Error('Please fill in at least one applicant')
    }

    var readunderstoodandagreed = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.readunderstoodandagreed).getValue()
    if (!readunderstoodandagreed) {
        throw new Error("Please tick the checkbox if the applicants has read, understood and agreed to the Personal Information Collection statement")
    }


}

PROXCardReqValidatinoSDK.validateAccessPeriod = () => {
    // const monthDiff = (d1, d2) => {
    //     let months;
    //     months = (d2.getFullYear() - d1.getFullYear()) * 12;
    //     months -= d1.getMonth();
    //     months += d2.getMonth();
    //     return months <= 0 ? 0 : months;
    // }

    const compare = (date1, date2) => {
        if (isNaN(date1) || isNaN(date2)) {
            throw new Error(date1 + " - " + date2);
        }
        return (date1 < date2) ? -1 : (date1 > date2) ? 1 : 0;

    }

    const IsWithinNMonth = (date1, date2, n) => {
        date1 = new Date(date1.setMonth(date1.getMonth() + n))
        return compare(date1, date2) == 1
    }

    let accessperiodfrom = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.accessperiodfrom).getValue()
    let accessperiodto = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.accessperiodto).getValue()


    // if (monthDiff(accessperiodfrom, accessperiodto) >= 6) {
    //     throw new Error("The Access Period should not exceed 6 months")
    // }

    if (!IsWithinNMonth(accessperiodfrom, accessperiodto, 6)) {
        throw new Error("The Access Period should not exceed 6 months.")
    }
}

PROXCardReqValidatinoSDK.checkIfAllC_CardAsync = async () => {
    var formid = ShareSdk.getCurrentFormId()
    var details = await ShareSdk.getAllPROXCardREQDTLsAsync(formid)

    return !details.some(p => p.clp_app_type != ShareSdk.PROXCardReqAPPType.EncodeC_Card)

}

PROXCardReqValidatinoSDK.checkSubstationsIsFilled = () => {

    //get count

    var count = Xrm.Page.getControl(ShareSdk.ProximityCardRequestSubGrids.Subgrid_station).getGrid().getTotalRecordCount();

    if (count === 0) {
        throw new Error('Please fill in Substation Access')
    }
}

PROXCardReqValidatinoSDK.checkErrorDetailsAsync = async () => {
    var formid = ShareSdk.getCurrentFormId()
    var details = await ShareSdk.getAllPROXCardREQDTLsAsync(formid)
    let errorMessages = []
    let accessperiodto = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.accessperiodto).getValue()
    let accessperiodfrom = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.accessperiodfrom).getValue()
    for (var item of details) {
        if (item.clp_app_type === ShareSdk.PROXCardReqAPPType.EncodeC_Card) {
            let errorMessage = PROXCardReqValidatinoSDK.validateExpiryDate(item.clp_name, item.clp_card_expiry_date, accessperiodto)
            if (errorMessage) {
                errorMessages.push(errorMessage)
            }
        }

        if (item.clp_app_type === ShareSdk.PROXCardReqAPPType.EncodeISMS) {
            var result = await ShareSdk.getTimeGapAsync(item._clp_proximity_card_value)

            if (result.accessperiodfrom_min) {
                var min_access_Date = new Date(result.accessperiodfrom_min)
                min_access_Date = new Date(min_access_Date.setDate(min_access_Date.getDate() - 1))

                var max_access_Date = new Date(result.accessperiodto_max)
                max_access_Date = new Date(max_access_Date.setDate(max_access_Date.getDate() + 1))

                if (min_access_Date > accessperiodto || accessperiodfrom > max_access_Date) {
                    errorMessages.push('The Access Period must be continous among Requests')
                }
            }

        }

        var stations = await ShareSdk.getPROXCardSubstationsAsync(formid)
        //duplicate detail checking
        var accessfromValue = ShareSdk.formateDateTime(accessperiodfrom, 'yyyy-MM-dd')
        var accesstoValue = ShareSdk.formateDateTime(accessperiodto, 'yyyy-MM-dd')
        for (var substation of stations) {

            var existsingDetails = await ShareSdk.getApplicantDeatilsAsync(item.clp_contractor_id, accessfromValue, accesstoValue, substation.clp_scadacode, formid)
            if (existsingDetails.length > 0) {
                errorMessages.push(`Duplicated submission for ${item.clp_contractor_id}. Please check. `)
            }
        }

    }

    const contractor_ids = details.map(p => p.clp_contractor_id).filter(i => i)

    if (Array.from(new Set(contractor_ids)).length != contractor_ids.length) {
        errorMessages.push('Applicant can exist only once within the request')
    }

    // if (details.length === 0) {
    //     errorMessages.push('Please fill in at least one application')
    // }

    return errorMessages
}

PROXCardReqValidatinoSDK.validateExpiryDate = (cardno, expiryDate, accessTo) => {

    if (!ShareSdk.PROXCardCheckCardExpiryAsync(cardno, expiryDate, accessTo)) {
        var dateStr = expiryDate.toLocaleDateString([], {
            year: 'numeric',
            month: '2-digit',
            day: '2-digit',
        })
        return `End date of Access Period cannot exceed the expiry date (${dateStr}) of C-Card`
    }
}

PROXCardReqValidatinoSDK.checkWarnningDetailsAsync = async () => {
    var formid = ShareSdk.getCurrentFormId()
    var details = await ShareSdk.getAllPROXCardREQDTLsAsync(formid)
    let warnningMessages = []
    for (var item of details) {
        let warnningMessage = await PROXCardReqValidatinoSDK.warnningDetailCheckAsync(item.clp_app_type, item.clp_card_no, formid, item._clp_proximity_card_applicant_value)
        if (warnningMessage.length > 0) {
            warnningMessages = [...warnningMessages, ...warnningMessage]
        }
    }

    return warnningMessages
}

PROXCardReqValidatinoSDK.warnningDetailCheckAsync = async (appType, cardno, formid, applicantid) => {

    let warnningMessages = []
    if (appType === ShareSdk.PROXCardReqAPPType.NewISMS) {
        if (applicantid) {
            let warnningMessage = await PROXCardReqValidatinoSDK.checkDuplicateApplicationsAsync(formid, applicantid)
            if (warnningMessage) {
                warnningMessages.push(warnningMessage)
            }

            let warnningMessage2 = await PROXCardReqValidatinoSDK.checkISMSCardsByHolderAsync(cardno, applicantid)
            if (warnningMessage2) {
                warnningMessages.push(warnningMessage2)
            }
        }
        else if (appType === ShareSdk.PROXCardReqAPPType.EncodeC_Card) {
            let warnningMessage = await PROXCardReqValidatinoSDK.checkCCardByHolderAsync(cardno, applicantid)
            if (warnningMessage) {
                warnningMessages.push(warnningMessage)
            }
        }
    }


    return warnningMessages
}

/**
 * 'More than one "New ISMS" applied checking
 */
PROXCardReqValidatinoSDK.checkDuplicateApplicationsAsync = async (formid, applicantid) => {

    var result = await PROXCardReqValidatinoSDK.getApplicantsforNewISMSAsync(formid, applicantid)
    if (result.length > 0) {
        // `const appnos = result.filter(p => p.clp_proximity_card_request).map(p => p.clp_proximity_card_request.clp_name).join()
        // if (appnos.length > 0) {
        //     return `Applicant has applied 'New ISMS' in the following request: ${appnos} `
        // }`

        const appnos = result.map(p => p["request.clp_name"]).join()
        if (appnos.length > 0) {
            return `Applicant has applied 'New ISMS' in the following request: ${appnos} `
        }
    }
}

//
PROXCardReqValidatinoSDK.checkISMSCardsByHolderAsync = async (cardno, applicantid) => {
    var result = await PROXCardReqValidatinoSDK.getHoldingCardsAsync(cardno, applicantid)
    if (result.length > 0) {
        const cardnos = result.map(p => p.clp_name).join()
        return `Applicant is holding the following card(s): ${cardnos} `
    }
}

PROXCardReqValidatinoSDK.checkCCardByHolderAsync = async (cardno, applicantid) => {
    var result = await PROXCardReqValidatinoSDK.getAssociatedCardsByapplicantAsync(cardno, applicantid)
    if (result.length === 0) {
        var holdCards = await PROXCardReqValidatinoSDK.getHoldingCardsAsync(cardno, applicantid)
        if (holdCards.length > 0) {
            const cardnos = holdCards.map(p => p.clp_name).join()
            return `Applicant is holding the following card(s): ${cardnos} `
        }

    }
}

PROXCardReqValidatinoSDK.getAssociatedCardsByapplicantAsync = (cardno, applicantid) => {

    var fetchXml = `<fetch version="1.0" output-format="xml-platform" mapping="logical" >
    <entity name="clp_proximitycardinventory" >
        <attribute name="clp_name" />
        <filter type="and" >
            <condition attribute="clp_name" operator="eq" value="${cardno}" />
            <condition attribute="clp_holder" operator="eq" value="${applicantid}" />
        </filter>
        <link-entity name="clp_proximitycardapplicant" to="clp_proximitycardinventoryid" from="clp_associated_card" link-type="inner" >
            <filter type="and" >
                <condition attribute="clp_proximitycardapplicantid" operator="eq" value="${applicantid}" />
            </filter>
        </link-entity>
    </entity>
</fetch>`
    return fetch(`/api/data/v9.0/clp_proximitycardinventories?fetchXml=${encodeURI(fetchXml)}`,
        {
            headers: {
                "Content-Type": "application/json",
                'Prefer': 'odata.include-annotations="*"',
            }
        }
    ).then(res => res.json()).then(res => {

        if (res.error) {
            throw new Error(res.error.message);
        }

        return Promise.resolve(res.value)


    }).catch(err => {
        console.error(err)
        throw err;
    })
}


PROXCardReqValidatinoSDK.getHoldingCardsAsync = (cardno, applicantid) => {

    const condition = ` clp_name ne '${cardno}' and _clp_holder_value eq '${applicantid}'`
    return fetch(`/api/data/v9.0/clp_proximitycardinventories?$filter=${condition}`,
        {
            headers: {
                "Content-Type": "application/json",
                'Prefer': 'odata.include-annotations="*"',
            }
        }
    ).then(res => res.json()).then(res => {

        if (res.error) {
            throw new Error(res.error.message);
        }

        return Promise.resolve(res.value)


    }).catch(err => {
        console.error(err)
        throw err;
    })
}

PROXCardReqValidatinoSDK.getApplicantsforNewISMSAsync = (requestGuid, applicantid) => {

    // filterstatus = [ShareSdk.PROXCardReqStatus.RequestCompleted,
    // ShareSdk.PROXCardReqStatus.RequestCancelled]
    // const wfcondition = `clp_proximitycardrequestid ne '${requestGuid}' and clp_workflow_status ne null and ` + filterstatus.map(p => ` clp_workflow_status ne ${p} `).join(' and ')

    // const condition = `clp_app_type eq ${ShareSdk.PROXCardReqAPPType.NewISMS} and _clp_proximity_card_applicant_value eq '${applicantid}'`
    // return fetch(`/api/data/v9.0/clp_proximity_card_request_details?$filter=${condition}&$expand=clp_proximity_card_request($filter=${wfcondition})`,
    //     {
    //         headers: {
    //             "Content-Type": "application/json",
    //             'Prefer': 'odata.include-annotations="*"',
    //         }
    //     }
    // ).then(res => res.json()).then(res => {

    //     if (res.error) {
    //         throw new Error(res.error.message);
    //     }

    //     return Promise.resolve(res.value)


    // }).catch(err => {
    //     console.error(err)
    //     throw err;
    // })

    var fetchXml = `<fetch  version="1.0" output-format="xml-platform" mapping="logical">
  <entity name="clp_proximity_card_request_detail">
    <filter>
      <condition attribute="clp_app_type" operator="eq" value="768230000" />
      <condition attribute="clp_proximity_card_applicant" operator="eq" value="${applicantid}" />
    </filter>
    <link-entity name="clp_proximitycardrequest" from="clp_proximitycardrequestid" to="clp_proximity_card_request" alias="request">
      <attribute name="clp_name" />
      <filter>
        <condition attribute="clp_proximitycardrequestid" operator="ne" value="${requestGuid}" />
        <condition attribute="clp_workflow_status" operator="not-null" />
        <condition attribute="clp_workflow_status" operator="not-in">
          <value>${ShareSdk.PROXCardReqStatus.RequestCompleted}</value>
          <value>${ShareSdk.PROXCardReqStatus.RequestCancelled}</value>
        </condition>
      </filter>
    </link-entity>
  </entity>
</fetch>`
    return fetch(`/api/data/v9.0/clp_proximity_card_request_details?fetchXml=${encodeURI(fetchXml)}`,
        {
            headers: {
                "Content-Type": "application/json",
                'Prefer': 'odata.include-annotations="*"',
            }
        }
    ).then(res => res.json()).then(res => {

        if (res.error) {
            throw new Error(res.error.message);
        }

        return Promise.resolve(res.value)


    }).catch(err => {
        console.error(err)
        throw err;
    })
}

PROXCardReqValidatinoSDK.substationProcessAsync = async () => {
    const formid = ShareSdk.getCurrentFormId()

    const wecnt = await ShareSdk.getSubstationsCountAsync(formid, ShareSdk.SubstationCategory.PC, ShareSdk.Region.East_West)
    const northcnt = await ShareSdk.getSubstationsCountAsync(formid, ShareSdk.SubstationCategory.PC, ShareSdk.Region.North)
    Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.ss_we).setValue(wecnt)
    Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.ss_nr).setValue(northcnt)

    Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.nosofss).setValue(`${wecnt + northcnt}`)

    var stations = await ShareSdk.getPROXCardSubstationsAsync(formid)

    stationcodes = stations.map(p => p.clp_scadacode).join()
    Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.ss_display).setValue(stationcodes)
    var fulllist = await ShareSdk.getStationFullListAsync(formid, ShareSdk.SubstationCategory.PC)
    Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.ss_list).setValue(fulllist.join())
}

PROXCardReqValidatinoSDK.detailsDataProcessAsync = async () => {
    var formid = ShareSdk.getCurrentFormId()
    var details = await ShareSdk.getAllPROXCardREQDTLsAsync(formid)
    const user = await ShareSdk.getCurrentClpUserAsync()
    for (var item of details) {
        var cards = await ShareSdk.getPROXCardByCardNoAsync(item.clp_card_no)

        card = cards[0]


        if (card) {

            //should save data after user submitting the request
            //Update C-Card Expiry Date
            if (item.clp_app_type === ShareSdk.PROXCardReqAPPType.EncodeC_Card) {

                if (card.clp_card_expiry_date != item.clp_card_expiry_date) {

                    await ShareSdk.updateCardExpiryDateAsync(card.clp_proximitycardinventoryid, item.clp_card_expiry_date, user?.clp_userid)

                }
            }
        } else {

            let applicantid = ''
            if (!item.clp_proximity_card_applicant) {
                //Insert Applicant if it does not exist

                var company = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.lut_ctrcomp).getValue()
                var existsApplicants = await ShareSdk.getApplicantByNameCTRIDANDCompanyAsync(item.clp_applicant_name, item.clp_contractor_id, item.clp_phone_no, ShareSdk.getLookupName(company))
                if (existsApplicants.length > 0) {
                    applicant = existsApplicants[0]
                    applicantid = applicant.clp_proximitycardapplicantid
                } else {
                    applicant = await ShareSdk.addApplicantAsync(item.clp_applicant_name, item.clp_contractor_id, item.clp_phone_no, ShareSdk.getLookupName(company))
                    applicantid = applicant.clp_proximitycardapplicantid
                }

                await ShareSdk.updatePROXCardREQDTLAsync(item.clp_proximity_card_request_detailid, applicantid)
            } else {
                applicantid = item.clp_proximity_card_applicant.clp_proximity_card_applicantid
            }


            if (item.clp_app_type === ShareSdk.PROXCardReqAPPType.EncodeC_Card) {

                //should save data after user submitting the request


                //Insert C-Card to inventory if it does not exist
                var newCard = await ShareSdk.addNewCardAsync(item.clp_card_no, ShareSdk.ProximityCardCardType.ContractorConsultantPass, ShareSdk.ProximityCardStatus.Allocated,
                    item.clp_card_expiry_date, ShareSdk.IssueDepartmentDefinition.NA, applicantid, null, user?.clp_userid)

                await ShareSdk.updatePROXCardREQDTLAsync(item.clp_proximity_card_request_detailid, applicantid, newCard.clp_proximitycardinventoryid, user?.clp_userid)
            }
        }


    }
}

PROXCardReqValidatinoSDK.validateAlcCardsAsync = async () => {
    var formid = ShareSdk.getCurrentFormId()
    var details = await ShareSdk.getAllPROXCardREQDTLsAsync(formid)
    for (var item of details) {
        if (!item.clp_is_rejected) {
            if (!item._clp_proximity_card_value) {
                throw new Error('Please allocate all the cards')
            }
        }
    }
}


PROXCardReqValidatinoSDK.generateExpiryWeekdayDate = () => {

    // const isWeekDay = (date) => date.getDay() % 6 !== 0
    var accessto = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.accessperiodto).getValue()

    var loop = new Date(accessto)

    let find = false
    let count = 0
    while (!find) {

        var newDate = new Date(loop.setDate(loop.getDate() - 1))

        count++
        if (count === 3) {
            Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.expiry_3_wd).setValue(newDate)
        }
        else if (count === 7) {

            Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.expiry_5_wd).setValue(newDate)
        } else if (count === 10) {

            Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.expiry_10_wd).setValue(newDate)
            find = true
        }

        loop = newDate;
    }

}

PROXCardReqValidatinoSDK.getTimeGapAsync = (cardid) => {
    var fetchXml = `<fetch version="1.0" output-format="xml-platform" mapping="logical" aggregate="true" >
    <entity name="clp_proximity_card_request_detail" >
        
        <filter type="and" >
            <condition attribute="clp_proximity_card" operator="eq" value="${cardid}" />
            <condition attribute="clp_detail_status" operator="in" >
                <value>
                    ${ShareSdk.PROXCardIndividualStatus.WaitingforCardCollection}
                </value>
                <value>
                    ${ShareSdk.PROXCardIndividualStatus.Issued}
                </value>
            </condition>
        </filter>
        <link-entity name="clp_proximitycardrequest" to="clp_proximity_card_request" from="clp_proximitycardrequestid" link-type="inner" >
            <attribute name="clp_accessperiodfrom" aggregate="min" alias="accessperiodfrom_min" />
            <attribute name="clp_accessperiodto" aggregate="max" alias="accessperiodto_max" />
            <filter type="and" >
                <condition attribute="clp_workflow_status" operator="eq" value="${ShareSdk.PROXCardReqStatus.RequestCompleted}" />
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
        return Promise.resolve(res.value[0])
    }).catch(err => {
        console.error(err)
        throw err;
    })
}

