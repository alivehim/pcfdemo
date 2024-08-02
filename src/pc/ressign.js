var PROXCardReqReassignSDK = window.PROXCardReqReassignSDK || {};
var ctx = Xrm.Page.ui.formContext;

async function isGivenApprover() {
    let givenApproverid = ctx.getAttribute("clp_chosenapprover").getValue()[0].id.replace(/[{}]*/g, "").toLowerCase();
    let userId = ShareSdk.getCurrentUserId();
    let clp_user_email = await getClpUserEmail(givenApproverid);
    let user_email = await getUserEmail(userId);
    return clp_user_email === user_email;
}

async function getUserEmail(id) {
    return Xrm.WebApi.retrieveRecord("systemuser", id).then(function (data) {
        if (data['internalemailaddress']) {
            return data["internalemailaddress"]
        } else {
            return "not found"
        }
    }, function (e) {
        console.error(e);
    });
}

async function getClpUserEmail(id) {
    return Xrm.WebApi.retrieveRecord("clp_user", id).then(function (data) {
        if (data['clp_emailaddress']) {
            return data["clp_emailaddress"]
        } else {
            return "not found"
        }
    }, function (e) {
        console.error(e);
    });
}

function updateRequestApprover(approver_id, request_id) {
    let a_id = approver_id.replace(/[{}]*/g, "").toLowerCase();
    let r_id = request_id.replace(/[{}]*/g, "").toLowerCase()
    let data = {
        "clp_ChosenApprover@odata.bind": "/clp_users(" + a_id + ")"
    }
    console.log("update request", data, a_id, r_id);
    Xrm.WebApi.updateRecord("clp_proximitycardrequest", r_id, data).then(function (result) {
        console.log("request:", result);

    }, function (e) {
        console.error(e);
    });
}

function insertEmailRequest(approver, request_name) {

    let data = {
        "clp_email_template_name": "SSMS_PC_WaitingforApproval",
        "clp_email_to": approver,
        "clp_name": `EmailEvent-${request_name}`,
    }

    console.log("email request: ", approver, request_name, data);

    Xrm.WebApi.createRecord("clp_proximitycard_event", data).then(
        function success(result) {
            console.log("Account created ", result);
            // perform operations on record creation
        },
        function (error) {
            console.log(error.message);
            // handle error conditions
        }
    );

}

PROXCardReqReassignSDK.onAction = async () => {
    // Xrm.Utility.showProgressIndicator("Loading")
    // console.log("is equal:", ShareSdk.isPROXCardRequestor());

    // 判断是不是given approver
    let isApprover = await isGivenApprover();

    // 判断是不是admin或given approver
    let isAdmin = ShareSdk.isProximityCardAdmin();

    // if (!isAdmin) {
    //     window.parent.ShareSdk.showGlobalNotification("you are not at right position to reassign", 2);
    //     return false;
    // }
    var lookupOptions = {
        defaultEntityType: "clp_user",
        entityTypes: ["clp_user"],
        allowMultiSelect: false
    };
    // approver reassign
    if (!isAdmin) {
        // if (true) {
        let r_pickup = Xrm.Utility.lookupObjects(lookupOptions).then(
            function (success) {
                // Xrm.Utility.closeProgressIndicator();
                console.log("success callback: ", success);
                if (success.length > 0) {
                    console.log("clp user", success[0]);
                    try {
                        // TODO 这里要直接web api改不能直接存form
                        let lookupValue = [];
                        let lookupObject = success[0];
                        lookupValue.push(lookupObject);
                        let control = ctx.getAttribute("clp_chosenapprover");
                        control.setValue(lookupValue);
                        let id = ctx.data.entity.getId();
                        updateRequestApprover(lookupObject.id, id);
                        // ctx.data.entity.save();
                    } catch (e) {
                        console.log("lookup failed", e)
                    }
                    return success[0];
                } else {
                    return success
                }

            }).then(async function (success) {
            console.log("success:", success);
            let clp_user_email = await getClpUserEmail(success.id);
            let request_name = ctx.getAttribute("clp_name").getValue();
            insertEmailRequest(clp_user_email, request_name);
        }).catch(function (e) {
            console.log(e);
        });
        // r_pickup.then(getRequestDetail);
    } else {
        lookupOptions.defaultViewId = "c17afee7-bf84-ed11-81ac-000d3a07ca3c".toUpperCase();
        // lookupOptions.viewIds = ["c17afee7-bf84-ed11-81ac-000d3a07ca3c"];
        let r_pickup = Xrm.Utility.lookupObjects(lookupOptions).then(
            function (success) {
                // Xrm.Utility.closeProgressIndicator();
                console.log("success callback: ", success);
                if (success.length > 0) {
                    console.log("clp user", success[0]);
                    try {
                        let lookupValue = [];
                        let lookupObject = success[0];
                        lookupValue.push(lookupObject);
                        let control = ctx.getAttribute("clp_chosenapprover");
                        // TODO 这里要直接web api改不能直接存form
                        control.setValue(lookupValue);

                        // approverid+ request id
                        let id = ctx.data.entity.getId();
                        updateRequestApprover(lookupObject.id, id);
                        // ctx.data.entity.save();
                    } catch (e) {
                        console.log("lookup failed", e)
                    }
                    return success[0];
                } else {
                    return success
                }

            }).then(async function (success) {
            console.log("success:", success);
            let clp_user_email = await getClpUserEmail(success.id);
            let request_name = ctx.getAttribute("clp_name").getValue();
            insertEmailRequest(clp_user_email, request_name);
        }).catch(function (e) {
            console.log(e);
        });;
    }

}

PROXCardReqReassignSDK.enableButton = () => {
    if (!ShareSdk.IsProxRequestForm()) {
        return false
    }

    var status = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).getValue()
    var locked = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.is_locked).getValue()

    if (ShareSdk.isProximityCardAdmin()) {
        return true
    }
    if (!locked && status === ShareSdk.PROXCardReqStatus.WaitingforApproval) {
        return ShareSdk.isPROXCardRequestor()
    }

    return false
}