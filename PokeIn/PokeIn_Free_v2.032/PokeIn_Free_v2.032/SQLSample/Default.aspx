<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SQLSample._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>PokeIn SQL Demo</title>
    <script src="http://www.sheetsdk.com/Release/Latest.php" type="text/javascript" ></script>
    <script type="text/javascript" src="Handler.aspx?ms=connect" ></script>
</head>
<body onload='Start()'>
 <script>

     /*
        In this sample we used SheetSDK client side library to
        simplify actions on the browser side.
        
        For further information, please visit: http://sheetsdk.codeplex.com
     */

     //SheetSDK Main function
     function Main(APP) {
 	        //Create Web Form
            var frm = new WebForm(APP, "Form1");
            document.frm = frm;
 	        frm.Title("PokeIn SQL");

            //Create Label Control for Tables
 	        var lblTables = new WebLabel("lblTables");
 	        frm.AddControl(lblTables);
 	        lblTables.MoveTo(10, 10);
 	        lblTables.ResizeTo(170, 20);
 	        lblTables.Text('Tables');
 	        
 	        //Create ListBox Control For Tables
 	        var lstTables = new WebListBox("lstTables");
 	        frm.AddControl(lstTables);
 	        lstTables.MoveTo(10, 30);
 	        lstTables.ResizeTo(200, 260);

            //Create Label Control for Records
 	        var lblRecords = new WebLabel("lblRecords");
 	        frm.AddControl(lblRecords);
 	        lblRecords.MoveTo(220, 10);
 	        lblRecords.ResizeTo(170, 20);
 	        lblRecords.Text('Records');

            //Create ListBox control for Records
 	        var lstRecords = new WebListBox("lstRecords");
 	        frm.AddControl(lstRecords);
 	        lstRecords.MoveTo(220, 30);
 	        lstRecords.ResizeTo(450, 260);
 	        lstRecords.Style.font = "10px normal normal sans,Arial";
 	        
 	        //Create Label Control for Status
 	        var lblStatus = new WebLabel("lblStatus");
 	        frm.AddControl(lblStatus);
 	        lblStatus.MoveTo(10,295);
 	        lblStatus.ResizeTo(200,20); 
 	 
 	        //Show Form
 	        frm.MoveTo(100, 100);
 	        frm.ResizeTo(685, 340);
 	        frm.Show();

 	        SetStatus("Loading Tables..");
 	        //CALL Server Side Method via PokeIn
 	        pCall['SQL'].GetTables();

            //If selected index changed for Control ListBox
 	        lstTables.OnSelectedIndexChanged = function(sender) {
 	            var item = lstTables.GetSelectedItem();
 	            
 	            //CALL Server Side Method
 	            pCall['SQL'].GetRecords(item.value, 100);
 	        }; 
 	    };
 	    
 	    function SetStatus(mess)
 	    {
 	        var lblStatus = document.frm.Controls["lblStatus"];
 	        lblStatus.Text(mess);
 	    };
 	    
 	    //Response function for GetTables Methods
 	    function FillTable(table_arr)
 	    {
 	        var lstTables = document.frm.Controls["lstTables"];
 	        lstTables.ClearItems();
 	        for(var o in table_arr)
 	        {
 	            lstTables.AddItem(table_arr[o], table_arr[o]);
 	        };
 	        SetStatus("Tables Loaded");
 	    }

 	    //Response function for GetRecords Methods
 	    function FillRecords(record_arr) {
 	        var lstRecords = document.frm.Controls["lstRecords"];
 	        lstRecords.ClearItems();
 	        for (var o in record_arr) {
 	            lstRecords.AddItem(record_arr[o], record_arr[o]);
 	        };
 	        SetStatus("Records Loaded");
 	    }

 	    function Start() {
 	        WebApplication.Add("Appl", Main);
 	        WebApplication.GetApplication("Appl").Run();
 	    }

 	    //Lets Start PokeIn
 	    document.OnPokeInReady = function() {
 	        PokeIn.Start(function(status) {
 	            if (status) // started properly?
 	            {
 	                
 	            }
 	        });
 	    }
 	</script>
</body>
</html>
