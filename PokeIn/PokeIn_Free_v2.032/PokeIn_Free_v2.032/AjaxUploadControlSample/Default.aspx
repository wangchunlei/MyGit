<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="AjaxUploadControlSample._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>PokeIn Ajax Upload Demo</title>
    <script type="text/javascript" src="Handler.aspx?ms=connect"></script>
</head>
<body style='font:normal normal 12px sans,"Arial";'>

    <div style='width:400px;height:350px;'>
        <div id='UploadControl' style='width:240px;height:26px;float:left;'></div>
        <input type='button' value='Upload' id='UploadButton' />
        <p style='clear:both' />
        <p style='width:398px;height:300px;background-color:Gray;'>
            <img style='border:none;' id='Img' />
        </p>
        <br />
        <input type="button" id="DeleteFiles" value="Delete Files In Server Folder" />
        <br />
        <hr />
        <p style='font-size:80%;color:#336699;'>PokeIn Library. <a href='http://pokein.com' target=_blank>http://pokein.com</a> </p>
    </div>
    <script>
            var connected = false;
            var uploadControl = null;
            
            //Start PokeIn
            document.OnPokeInReady = function() {
                PokeIn.Start(function(is_done) {// Connected?
			            if (is_done) {
			                connected = is_done;
			                DrawUploadControl();
			            }
			        }
			    );
            };

            function DrawUploadControl() {
                var control = document.getElementById("UploadControl");
                
                uploadControl = PokeIn.CreateUploadControl(
                        {
                            targetElement: control, //document element to implement upload control
                            bgColor: "white", //iframe background color
                            fontColor: "#b00000", //font color
                            size: //control render size
                            {
                                width: 240, 
                                height: 26
                            },
                            messages: //custom messages
                            {
                                OnUpload: 'Uploading...',
                                OnSuccess: 'Done!',
                                OnFinalize: 'Finalizing...',
                                OnFail: 'Upload Failed!',
                                OnFileNotSelected: 'Select a File', 
                                OnInvalidFileType: 'Invalid file type' 
                            },
                            FileTypes: ["gif", "jpg", "jpeg", "bmp"] //accepted file types
                        }
                    );

                    uploadControl.OnUploadCompleted = function(name, fileName) {
                        /*
                        Do Whatever you want.
                        name :: name of the control object
                        fileName :: name of the uploaded file
                        */
                    };

                    uploadControl.OnUploadStart = function(name, fileName) {
                        /*
                        Do Whatever you want.
                        name :: name of the control object
                        fileName :: name of the uploaded file
                        */
                    };

                    var btnUpload = document.getElementById('UploadButton');

                    btnUpload.onclick = function() {
                        try {
                            //Start Upload
                            uploadControl.Start();
                        }
                        catch (err) {   //Exception Management
                            if (err == "FileNotSelected") {
                                alert('select the file');
                            }
                            else if (err == "InvalidFileType") {
                                alert('Supported File Types Are Limited To : gif, jpg, jpeg, bmp');
                            }
                        }
                    };
                } 

            var rnd = 0;
            var ImgElement = document.getElementById("Img");
            
            function ShowImage(imageName) {
                rnd++;
                ImgElement.src = "Handler.aspx?name=" + imageName + "&type=image&rnd=" + rnd++; //avoid browser caching
            }

            var deleteButton = document.getElementById("DeleteFiles");

            deleteButton.onclick = function() {
                //Call Server Side Method
                pCall['Dummy'].DeleteFilesInFolder();
            };

            function FilesDeleted(fileCount) {
                alert(fileCount + " file(s) deleted");
            };
            
            function Error(err) {
                alert("Error :" + err + " Occurred On File Delete");
            };
        
    </script>
</body>
</html>
