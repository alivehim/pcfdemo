
window.EAAAttachmentSdk = window.EAAAttachmentSdk || {}


/**
 * !!!
 * 1. Before use this function. Please rename the attachent section to "section_Attachment"
 * !!!
 * 
 * Triggered when form onLoad.
 * Replace the default Attachment list control to custom control.
 */
EAAAttachmentSdk.replaceAttachmentControlOnLoad = (info) => {
    const selector = () => window.parent.document.querySelector(`section[data-id='${info.id}']>div>div`)
    EAAShareSdk._doUntil(
        async () => {
            const target = selector()

            const currentUserId = EAAShareSdk.getCurrentUserId()

            const timelineid = Xrm.Page.data.entity.attributes.getByName("aia_eaa_timelineid")?.getValue()
            const requestorId = Xrm.Page.data.entity.attributes.getByName("ownerid").getValue()?.[0]?.id.replace(/[\{\}]*/g, "").toLowerCase()

            /**
             * UploadEnable for different role.
             */
            // var uploadFileEnable = assignedToUser == currentUserId && [SupportingDocumentSdk._statusType.PendingLBUFinanceApproval, SupportingDocumentSdk._statusType.PendingGFSR2RApproval].includes(status);

            /**
             * For global control, can user edit attachment (upload and delete) or not
             */
            const getGlobalEnable = async () => {

                if (info.type == EAAShareSdk.AttachmentType.SupportingDoc) {
                    const status = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.form_status).getValue()
                    if ([EAAShareSdk.FormStatus.Initiator_Pending, EAAShareSdk.FormStatus.Initiator_ResubmissionPending].includes(status) && currentUserId === requestorId) {
                        return true
                    }

                    return EAAShareSdk.IsAdmin()
                } else if (info.type == EAAShareSdk.AttachmentType.ApprovalEvidence) {
                    return true
                }
                return false
            }

            const globalEnable = await getGlobalEnable()


            EAAAttachmentSdk._renderAttachmentsControl(target, {
                multiple: true,
                uploadFunc: (file, attachmentType, timelineid) => {
                    if (attachmentType === EAAShareSdk.AttachmentType.SupportingDoc) {
                        requestId = EAAShareSdk.getFormId()
                    } else {
                        var replaceSymbol = /(\w*){(.*)}(.*)/g

                        if (Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.ApprovalFields.eaa_form)) {
                            formId = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.ApprovalFields.eaa_form).getValue()
                            requestId = formId[0].id.replace(replaceSymbol, "$1$2");
                        } else {
                            requestId = EAAShareSdk.getFormId()
                        }
                    }

                    var data = {
                        "aia_sys_eaa_form@odata.bind": `/aia_eaa_forms(${requestId})`,
                        aia_attachment_type: attachmentType,
                        // "aia_UploadedBy@odata.bind": `/systemusers(${Xrm._globalContext.userSettings.userId.replace(/[{}]*/g, "")})`
                    };

                    if (timelineid) {
                        data.aia_eaa_timelineid = timelineid
                    }

                    // need to create a record first
                    return fetch(
                        `/api/data/v9.0/${EAAShareSdk.Tables.form_attachment}s`,
                        {
                            method: "POST",
                            headers: {
                                "Content-Type": "application/json",
                                "Prefer": "return=representation"
                            },
                            body: JSON.stringify(data)
                        }
                    ).then(res => res.json()).then(res => {
                        if (res.error) {
                            throw new Error(res.error.message)
                        }
                        // upload file to this record
                        // should set the required filename when upload but not download
                        // RequestName is "xxxx-xxxx", it has 9 charaters
                        const fileName = `${file.name.slice(0, 250 - 9 - 3)}`
                        return fetch(
                            `/api/data/v9.0/${EAAShareSdk.Tables.form_attachment}s(${res.aia_eaa_form_attachmentid})/aia_eaa_attachment?x-ms-file-name=${fileName}`,
                            {
                                method: "PATCH",
                                headers: {
                                    "Content-Type": "application/octet-stream",
                                    "Accept": "application/json",
                                },
                                body: file,
                            }
                        ).then(async res => {
                            if (!(res.status === 204 || res.status === 200)) {
                                // throw new Error(`Upload file ${file.name} failed. Got error status code ${res.status}.`)
                                await res.json().then(res => {
                                    throw new Error(res.error.message)
                                })
                            }

                            return Promise.resolve()
                        })
                    }).catch(err => {
                        Xrm.UI.addGlobalNotification({
                            level: 2,
                            message: err.message,
                            type: 2,
                            showCloseButton: true
                        })
                    })
                },
                removeFunc: (fileItem) => {
                    return fetch(
                        `/api/data/v9.0/${EAAShareSdk.Tables.form_attachment}s(${fileItem.id})`,
                        {
                            method: "DELETE",
                            headers: {
                                "Prefer": "return=representation"
                            }
                        }
                    ).then(async res => {
                        if (res.status !== 204) {
                            const { error } = await res.json()
                            throw new Error(error.message)
                        }
                        return Promise.resolve()
                    }).catch(err => {
                        Xrm.UI.addGlobalNotification({
                            level: 2,
                            message: err.message,
                            type: 2,
                            showCloseButton: true
                        })
                    })
                },
                getFileList: (attachmentType, timelineid) => {

                    if (attachmentType === EAAShareSdk.AttachmentType.SupportingDoc) {
                        requestId = EAAShareSdk.getFormId()
                    } else {
                        var replaceSymbol = /(\w*){(.*)}(.*)/g
                        if (Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.ApprovalFields.eaa_form)) {
                            formId = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.ApprovalFields.eaa_form).getValue()
                            requestId = formId[0].id.replace(replaceSymbol, "$1$2");
                        } else {
                            requestId = EAAShareSdk.getFormId()
                        }

                    }

                    if (requestId == null || requestId == undefined || requestId == '') {
                        return Promise.resolve([]);
                    }
                    let extracondition = timelineid ? ` and aia_eaa_timelineid eq ${timelineid} ` : ''
                    return fetch(`/api/data/v9.0/${EAAShareSdk.Tables.form_attachment}s/?$filter=_aia_sys_eaa_form_value eq ${requestId} and aia_attachment_type eq ${attachmentType} ${extracondition}`).then(res => res.json()).then(res => {
                        if (res.error) {
                            throw new Error(res.error.message)
                        }

                        // if (attachmentType == EAAShareSdk.AttachmentType.SupportingDoc) {
                        //     // auto generate the correct fileName
                        //     requestName = Xrm.Page.data.entity.attributes.getByName("aia_name").getValue() || ""
                        // } else {
                        //     if (Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.ApprovalFields.eaa_form)) {
                        //         form = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.ApprovalFields.eaa_form).getValue()
                        //         requestName = form[0].name
                        //     }
                        //     else {
                        //         requestName = Xrm.Page.data.entity.attributes.getByName("aia_name").getValue() || ""
                        //     }

                        // }


                        /**
                         * get This attachment can be delete or not
                         * Only when attachment owner is current user.
                         * @param {Object} attachmentItem 
                         * @returns {Boolean} 
                         */
                        const getAttachmentDeleteEnable = (attachmentItem) => {
                            // pay attention that the id in Xrm is Upper case and the id in api in lower case
                            return attachmentItem._ownerid_value === currentUserId
                        }


                        return res.value.filter(i => i.aia_eaa_attachment).map(i => ({
                            fileName: i.aia_eaa_attachment_name || "",
                            id: i.aia_eaa_form_attachmentid,
                            url: `/api/data/v9.0/${EAAShareSdk.Tables.form_attachment}s(${i.aia_eaa_form_attachmentid})/aia_eaa_attachment/$value`,
                            data: i,
                            // sub control
                            deleteEnable: getAttachmentDeleteEnable(i)
                        }))
                    }).catch(err => {
                        Xrm.UI.addGlobalNotification({
                            level: 2,
                            message: err.message,
                            type: 2,
                            showCloseButton: true
                        })
                    })
                },
                uploadEnable: globalEnable,
                deleteEnable: globalEnable,
                attachmentType: info.type,
                timelineid: timelineid
            })
        },
        selector,
    )
}



/**
 * Reander a attachment list.
 * Pay attention that you should handle error 
 * 
 * @param {*} target The render target HTMLElement
 * @param {*} param1 
 * @param {Promise} uploadFunc A promise that upload a File
 * @param {Promise} removeFunc A promise remove a attachment.
 * @param {Promise} getFileList A promise that return the table items.
 * @param {Boolean} multiple Multiple files upload ?
 * @param {number} maxSize The max size allow user upload.
 * 
 * @param {Boolean} uploadEnable enable to upload file
 * @param {Boolean} deleteEnable enable to delete file. A global control, and a sub control is in getFileList function, the file item.
 */
EAAAttachmentSdk._renderAttachmentsControl = (target, {
    uploadFunc = () => { },
    removeFunc = () => { },
    getFileList = () => { },
    multiple = false,
    maxSize = 32 * 1024 * 1024,
    uploadEnable = false,
    deleteEnable = false,
    attachmentType = EAAShareSdk.AttachmentType.SupportingDoc,
    timelineid = null
}) => {
    window.React = window.parent.React
    window.ReactDOM = window.parent.ReactDOM
    window.FluentUIReact = window.parent.FluentUIReact


    const comp = () => {
        // Uploaded file List
        const [fileList, setFileList] = React.useState([])

        /**
         * A "Loading" control.
         * When loading... Will set a loading mask above this upload component
         */
        const loadingRef = React.useRef()
        const setLoading = (loading = false) => {
            const elementRef = loadingRef.current
            if (loading && elementRef) {
                elementRef.style.display = "flex"
            } else if (elementRef) {
                elementRef.style.display = 'none'
            }
        }

        const inputRef = React.useRef()
        /**
         * After user selected files
         */
        const fileSelected = async () => {
            let files = Array.from(inputRef.current.files)
            // reset input 
            inputRef.current.value = ""

            /**
             * files filter, prevent user from uploading the oversize files.
             */

            for (const i of files) {
                if (i.size > maxSize) {

                    let message = `The maximunm size of attachment is 32M, ${i.name} is over this limitation.`
                    const msgKey = new Date().getTime().toString(16)
                    Xrm.Page.ui.formContext.ui.setFormNotification(
                        message,
                        'ERROR',
                        msgKey);

                    window.setTimeout(function () {
                        Xrm.Page.ui.formContext.ui.clearFormNotification(msgKey);
                    }, 10000);
                }
                else if (i.size === 0) {
                    let message = `File size cannot be 0: ${i.name}`
                    const msgKey = new Date().getTime().toString(16)
                    Xrm.Page.ui.formContext.ui.setFormNotification(
                        message,
                        'ERROR',
                        msgKey);

                    window.setTimeout(function () {
                        Xrm.Page.ui.formContext.ui.clearFormNotification(msgKey);
                    }, 10000);
                }
            }

            files = files.filter(i => i.size != 0 && i.size <= maxSize)
            if (files.length > 0) {
                // upload files
                setLoading(true)
                await Promise.all(files.map(file => uploadFunc(file, attachmentType, timelineid)))
                await getSetFileList()
            }
            setLoading(false)
        }

        const deleteFile = (fileItem) => {
            setLoading(true)
            removeFunc(fileItem).then(() => {
                return getSetFileList()
            }).finally(() => {
                setLoading(false)
            })

        }

        const getSetFileList = () => {
            setLoading(true)
            return getFileList(attachmentType, timelineid).then((fileList = []) => {
                setFileList(fileList)
            }).finally(() => {
                setLoading(false)
            })
        }

        /**
         * download file 
         * @param {Object} fileItem
         */
        const downloadFile = (fileItem) => {
            setLoading(true)
            fetch(fileItem.url,).then(res => res.blob()).then(blobData => {
                const url = URL.createObjectURL(blobData)
                const a = document.createElement('a')
                a.href = url
                a.download = fileItem.fileName
                a.click()
                URL.revokeObjectURL(url)
            }).catch(err => {
                Xrm.UI.addGlobalNotification({
                    level: 2,
                    message: err.message,
                    type: 2,
                    showCloseButton: true
                })
            }).finally(() => {
                setLoading(false)
            })
        }

        React.useEffect(() => {
            getSetFileList()
        }, [])


        const loadingSvgSrc = URL.createObjectURL(new Blob([
            `
            <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink"
            style="margin: auto; display: block;" viewBox="0 0 100 100" preserveAspectRatio="xMidYMid">
            <circle cx="50" cy="50" fill="none" stroke="#d31145" stroke-width="10" r="35"
                stroke-dasharray="164.93361431346415 56.97787143782138">
                <animateTransform attributeName="transform" type="rotate" repeatCount="indefinite" dur="1s"
                    values="0 50 50;360 50 50" keyTimes="0;1"></animateTransform>
            </circle>
        </svg>
            `
        ], { type: "image/svg+xml" }))

        return (
            React.createElement(
                "div",
                {
                    style: {
                        width: "100%",
                        display: 'flex',
                        flexDirection: 'column',
                        position: "relative"
                    }
                },
                // loading mask
                React.createElement(
                    "div",
                    {
                        ref: loadingRef,
                        style: {
                            width: "100%",
                            height: "100%",
                            position: 'absolute',
                            top: "0",
                            left: "0",
                            display: "none",
                            alignItems: "center",
                            justifyContent: "center",
                            zIndex: "500",
                            pointerEvents: 'visiblefill',
                            backgroundColor: "rgba(0, 0, 0, 0.2)",
                        }
                    },
                    React.createElement(
                        "div",
                        {
                            style: {
                                backgroundImage: `url(${loadingSvgSrc})`,
                                backgroundPosition: "center",
                                backgroundRepeat: "no-repeat",
                                width: "30px",
                                height: "30px"
                            }
                        }
                    )
                ),
                React.createElement(
                    "input",
                    {
                        type: 'file',
                        onChange: fileSelected,
                        ref: inputRef,
                        style: {
                            display: "none"
                        },
                        multiple,
                    }
                ),
                (
                    uploadEnable ? React.createElement(
                        FluentUIReact.DefaultButton,
                        {
                            onClick: () => inputRef.current.click(),
                            style: {
                                width: "fit-content",
                                borderColor: "black",
                                // color: "#D31145",
                                marginBottom: "15px"
                            },
                            text: "Upload file"
                        }
                    ) : ""
                ),
                (
                    fileList.length === 0 ?
                        "No attachments uploaded." :
                        fileList.map(fileItem => {
                            return (
                                React.createElement(
                                    'div',
                                    {
                                        style: {
                                            display: "flex",
                                            alignItems: 'center'
                                        }
                                    },
                                    React.createElement(
                                        "button",
                                        {
                                            style: {
                                                fontSize: "16px",
                                                color: "#333D47",
                                                background: "transparent",
                                                border: 'none',
                                                textDecoration: "underline"
                                            },
                                            onClick: () => { downloadFile(fileItem) }
                                        },
                                        React.createElement(
                                            FluentUIReact.FontIcon,
                                            {
                                                iconName: "Attach",
                                                style: {
                                                    fontSize: 18,
                                                    height: 18,
                                                    width: 18,
                                                    cursor: "pointer"
                                                },
                                            }
                                        ),
                                        fileItem.fileName,
                                    ),
                                    (
                                        (deleteEnable && fileItem.deleteEnable) ? React.createElement(
                                            'button',
                                            {
                                                style: {
                                                    width: '56px',
                                                    height: '22px',
                                                    'line-height': '1px'
                                                },
                                                onClick: () => deleteFile(fileItem),
                                            },
                                            'Delete'
                                        ) : ""
                                    ),
                                    // (
                                    //     (deleteEnable && fileItem.deleteEnable) ? React.createElement(
                                    //         FluentUIReact.FontIcon,
                                    //         {
                                    //             iconName: "Delete",
                                    //             style: {
                                    //                 fontSize: 18,
                                    //                 height: 18,
                                    //                 width: 18,
                                    //                 cursor: "pointer",
                                    //                 color: "#D31145",
                                    //                 marginLeft: "20px"
                                    //             },
                                    //             onClick: () => deleteFile(fileItem)
                                    //         }
                                    //     ): ""
                                    // ),
                                )
                            )
                        })
                )
            )
        )
    }

    ReactDOM.render(
        React.createElement(React.StrictMode, {},
            React.createElement(comp)
        ),
        target
    )
}



