"use strict";(self.webpackChunkewarehouse=self.webpackChunkewarehouse||[]).push([[564],{4564:(st,y,c)=>{c.r(y),c.d(y,{LetterconfigurationModule:()=>nt});var _=c(96814),S=c(81303),t=c(65879);let Z=(()=>{class o{static#t=this.\u0275fac=function(n){return new(n||o)};static#e=this.\u0275cmp=t.Xpm({type:o,selectors:[["app-letterconfiguration"]],decls:1,vars:0,template:function(n,i){1&n&&t._UZ(0,"router-outlet")},dependencies:[S.lC]})}return o})();var U=c(13519),a=c.n(U),h=c(20553),m=c(39007),f=c(77874),g=c(56223),L=c(78889),b=c(66755),k=c(89515),I=c(84775),w=c(96226),O=c(63504);const q=["myckeditor"];function M(o,d){if(1&o&&(t.TgZ(0,"option",40),t._uU(1),t.qZA()),2&o){const e=d.$implicit;t.Q6J("value",e.intId),t.xp6(1),t.hij(" ",e.vchProcessName," ")}}function D(o,d){if(1&o&&(t.TgZ(0,"option",40),t._uU(1),t.qZA()),2&o){const e=d.$implicit;t.Q6J("value",e.typeId),t.xp6(1),t.hij(" ",e.typeName,"")}}function E(o,d){if(1&o&&(t.TgZ(0,"option",40),t._uU(1),t.qZA()),2&o){const e=d.$implicit;t.Q6J("value",e.typeId),t.xp6(1),t.hij(" ",e.typeName,"")}}function J(o,d){if(1&o){const e=t.EpF();t.TgZ(0,"div",41)(1,"div",12)(2,"div",10)(3,"label",13),t._uU(4),t.qZA(),t.TgZ(5,"div",14)(6,"select",19),t.NdJ("ngModelChange",function(i){t.CHM(e);const s=t.oxw();return t.KtG(s.selSignType=i)}),t.TgZ(7,"option",20),t._uU(8,"--Select--"),t.qZA(),t.YNc(9,E,2,2,"option",21),t.qZA()()()()()}if(2&o){const e=t.oxw();t.xp6(4),t.hij("*","Digital Sign Type"," "),t.xp6(2),t.Q6J("ngModel",e.selSignType),t.xp6(3),t.Q6J("ngForOf",e.getwaytypes)}}function R(o,d){if(1&o&&(t.TgZ(0,"tr")(1,"td"),t._uU(2),t.qZA(),t.TgZ(3,"td"),t._uU(4),t.qZA()()),2&o){const e=d.$implicit;t.xp6(2),t.Oqu(e.keyName),t.xp6(2),t.Oqu(e.keyLabelName)}}function P(o,d){if(1&o){const e=t.EpF();t.ynx(0),t.TgZ(1,"button",42),t.NdJ("click",function(){t.CHM(e);const i=t.oxw();return t.KtG(i.generateLetter())}),t._uU(2),t.qZA(),t.TgZ(3,"button",43),t.NdJ("click",function(){t.CHM(e);const i=t.oxw();return t.KtG(i.resetform())}),t._uU(4),t.qZA(),t.BQk()}2&o&&(t.xp6(2),t.Oqu("Submit"),t.xp6(2),t.Oqu("Reset"))}function H(o,d){if(1&o){const e=t.EpF();t.TgZ(0,"button",42),t.NdJ("click",function(){t.CHM(e);const i=t.oxw();return t.KtG(i.generateLetter())}),t._uU(1),t.qZA(),t.TgZ(2,"button",43),t.NdJ("click",function(){t.CHM(e);const i=t.oxw();return t.KtG(i.updateCancel())}),t._uU(3),t.qZA()}2&o&&(t.xp6(1),t.Oqu("Update"),t.xp6(2),t.Oqu("Cancel"))}let N=(()=>{class o{constructor(e,n,i,s,r,l,p,u,T){this.fb=e,this._location=n,this.route=i,this.router=s,this.commonService=r,this.vldChkLst=l,this.translate=p,this.encDec=u,this.LetterconfigService=T,this.title="Add Letter Configuration",this.jsonurl="assets/js/_configs/addLetter.config.json",this.name="Angular ",this.data="<p>Hello, world!</p>",this.isSelected=!1,this.itemID="0",this.selModuleName=0,this.selFormName=0,this.selLetterType=0,this.selSignType=0,this.txtLetterName=null,this.txtLetterContent=null,this.letterType=0,this.signStatus=0,this.txtFormId=null,this.txtEfminDate="",this.txtEtminDate="",this.letterID=0,this.letterlist=null,this.langKey="en",this.intViewManageRight=1,this.intaddRight=1,this.allFormsArr=[],this.userid=0,this.createId=0,this.previllage=3,this.username="",this.sessiontoken=sessionStorage.getItem("ADMIN_SESSION");let v=JSON.parse(this.sessiontoken);this.userid=v.USER_ROLE,this.createId=v.USER_ID,this.previllage=v.USER_PVLG}ngOnInit(){this.ckeConfig={allowedContent:!0,extraPlugins:"uploadimage",removeButtons:"exportPdf,Save,NewPage,Preview,Print,Templates,Cut,Copy,Paste,PasteFromWord,Scayt,Form,Checkbox,Radio,TextField,Textarea,Select,Button,ImageButton,HiddenField,Strike,Subscript,Superscript,CopyFormatting,RemoveFormat,Outdent,Indent,CreateDiv,Blockquote,BidiLtr,BidiRtl,Language,Anchor,Flash,Smiley,SpecialChar,Iframe,Maximize,ShowBlocks,About",disallowedContent:"*{*color}; *{*align}",filebrowserBrowseUrl:"https://ckeditor.com/apps/ckfinder/3.4.5/ckfinder.html",filebrowserImageBrowseUrl:"https://ckeditor.com/apps/ckfinder/3.4.5/ckfinder.html?type=Images",filebrowserUploadUrl:"http://172.27.30.93:7001/DMS_PHP/admin/ckEditorFileUpload",filebrowserImageUploadUrl:"http://172.27.30.93:7001/DMS_PHP/admin/ckEditorImageUpload"},this.minDate=new Date,this.maxDate=new Date;let e=this.router.snapshot.paramMap.get("id");if(""!=e){let i=this.encDec.decText(e).split(":");this.itemID=i[0],this.resubmitstatus=i[1],this.linkId=i[2],(""!=this.itemID||0!=this.itemID)&&this.getLetterinfo()}this.getAllFormName(),this.getFormKeys(),this.getGetwaytypes(),this.getLetterTypeList()}getAllFormName(){this.LetterconfigService.getAllForms([]).subscribe(n=>{let s=m.lW.from(n.RESPONSE_DATA,"base64");s=JSON.parse(s.toString()),200==s.status&&(this.allFormsArr=s.result)})}getGetwaytypes(){this.LetterconfigService.getGetwayType().subscribe(e=>{let n=e.RESPONSE_DATA;if(e.RESPONSE_TOKEN==f.HmacSHA256(n,h.N.apiHashingKey).toString()){let r=m.lW.from(n,"base64");r=JSON.parse(r.toString()),"200"==r.status?this.getwaytypes=r.result:console.log(r.messages)}else a().fire({icon:"error",text:"Something Went Wrong !!"})})}getLetterTypeList(){this.LetterconfigService.getLetterTypeData().subscribe(e=>{let n=e.RESPONSE_DATA;if(e.RESPONSE_TOKEN==f.HmacSHA256(n,h.N.apiHashingKey).toString()){let r=m.lW.from(n,"base64");r=JSON.parse(r.toString()),"200"==r.status?this.letterTypeList=r.result:console.log(r.messages)}else a().fire({icon:"error",text:"Something Went Wrong !!"})})}getFormKeys(){this.LetterconfigService.getConfigurationKeys({itemId:this.txtFormId}).subscribe(n=>{let i=n.RESPONSE_DATA;if(n.RESPONSE_TOKEN==f.HmacSHA256(i,h.N.apiHashingKey).toString()){let l=m.lW.from(i,"base64");l=JSON.parse(l.toString()),200==l.status?(this.keysArray=l.result,console.log(this.keysArray)):417==l.status?a().fire({icon:"error",text:"Invalid Request !!"}):a().fire({icon:"error",text:"Something Went Wrong !!"})}else a().fire({icon:"error",text:"Invalid Response !!"})})}gotoView(e){let n=this.encDec.encText(e.toString());this.route.navigate(["/admin/letterconfiguration/viewLetterConfig",n])}gotoadd(e){let n=this.encDec.encText(e.toString());this.route.navigate(["/admin/configuration/addLetterConfig",n])}generateLetter(){let e=0,n=this.selFormName,i=this.txtLetterName,s=this.letterType,r=this.signStatus,l=this.selSignType,p=this.encDec.escapeHtml(this.txtLetterContent);if(0==e&&!this.vldChkLst.BlankCheckRdoDropChk("selFormName",n,"Form Name")&&(e=1),0==e&&!this.vldChkLst.blankCheck("LetterName",i,"Letter Name Can Not Be Blank !!")&&(e=1),0==e&&!this.vldChkLst.isCharecterKey("LetterName",i,"Letter Name")&&(e=1),0==e&&!this.vldChkLst.minLength("LetterName",i,3,"Letter Name")&&(e=1),0==e&&!this.vldChkLst.maxLength("LetterName",i,32,"Letter Name")&&(e=1),0==e&&0==s&&(a().fire({icon:"error",text:"Please Select Letter Type !!"}),e=1),0==e&&1==r&&0==l&&(a().fire({icon:"error",text:"Please Select Sign Type !!"}),e=1),0==e&&(""==p||null==typeof p||null==p)&&(a().fire({icon:"error",text:"Please Enter Letter Content !!"}).then(function(){null!=p&&setTimeout(()=>{const u=document.getElementById("letterContent");u.focus(),u.scrollIntoView({behavior:"smooth",block:"center"})},500)}),e=1),console.log(p),0==e){let u={itemId:this.itemID,formId:n,letterType:this.letterType,lettername:i.trim(),letterContent:p,intCreatedBy:this.userId,intUpdatedBy:this.userId,SignTypeStatus:r,SignType:l,itemStatus:""};this.LetterconfigService.newLetter(u).subscribe(T=>{let v=T.RESPONSE_DATA;if(T.RESPONSE_TOKEN==f.HmacSHA256(v,h.N.apiHashingKey).toString()){let C=m.lW.from(v,"base64");C=JSON.parse(C.toString()),"200"==C.status?(a().fire({icon:"success",text:"Letter Created Successfully !!"}),this.resetform(),this.route.navigate(["/admin/letterconfiguration/viewLetterConfig"])):202==C.status?(a().fire({icon:"success",text:"Letter Updated Successfully !!"}),this.route.navigate(["/admin/letterconfiguration/viewLetterConfig"]),this.resetform()):417==C.status?a().fire({icon:"warning",text:"Unauthorized Response !!"}):a().fire({icon:"error",text:C.msg||"Something Went Wrong !!"})}else a().fire({icon:"error",text:"Invalid Response !!"})})}}getLetterinfo(){this.LetterconfigService.viewLetters({formId:"",LetterName:"",intLetterConfigId:this.itemID}).subscribe(n=>{let i=n.RESPONSE_DATA;if(n.RESPONSE_TOKEN==f.HmacSHA256(i,h.N.apiHashingKey).toString()){let l=m.lW.from(i,"base64");l=JSON.parse(l.toString()),200==l.status?(this.letterlist=l.result,this.letterlist.length>0&&(this.txtFormId=this.letterlist[0].intformId,this.selFormName=this.letterlist[0].intformId,this.txtLetterName=this.letterlist[0].vchLetterName,this.txtLetterContent=this.encDec.decodeHtml(this.letterlist[0].txtLetterContent),this.letterType=this.letterlist[0].intLetterType,this.signStatus=this.letterlist[0].tinSignTypeSts,this.selSignType=this.letterlist[0].intSignType)):417==l.status&&a().fire({icon:"error",text:"Invalid Request !!"})}else a().fire({icon:"error",text:"Invalid Response Message !!"})})}updateCancel(){this.route.navigate(["/admin/letterconfiguration/viewLetterConfig"])}resetform(){this.selFormName="0",this.txtLetterName=null,this.txtLetterContent=null,this.signStatus=0,this.selSignType=0}static#t=this.\u0275fac=function(n){return new(n||o)(t.Y36(g.qu),t.Y36(_.Ye),t.Y36(S.F0),t.Y36(S.gz),t.Y36(L.R),t.Y36(b._),t.Y36(k.sK),t.Y36(I.q),t.Y36(w.D))};static#e=this.\u0275cmp=t.Xpm({type:o,selectors:[["app-addletterconfiguration"]],viewQuery:function(n,i){if(1&n&&t.Gf(q,5),2&n){let s;t.iGM(s=t.CRH())&&(i.ckeditor=s.first)}},decls:85,vars:30,consts:[[1,"page-title"],["id","page-content",1,"page-controls-section"],[1,"card"],[1,"controls-section-header"],[1,"nav","nav-tabs"],[1,"nav-item"],["aria-current","page",1,"nav-link","active",3,"click"],[1,"nav-link",3,"click"],[1,"card-body"],[1,"controls-section"],[1,"row"],[1,"col-md-6"],[1,"form-group"],[1,"col-md-6","col-lg-4"],[1,"col-md-6","col-lg-6"],["id","selFormName",1,"form-select",3,"ngModel","ngModelChange"],["value","0"],[3,"value",4,"ngFor","ngForOf"],["type","text","id","LetterName","maxlength","50",1,"form-control",3,"ngModel","ngModelChange","keypress","keyup"],[1,"form-select",3,"ngModel","ngModelChange"],["value","0","disabled","","selected",""],[3,"value",4,"ngFor","ngForIndex","ngForOf"],[1,"form-check","form-check-inline"],["name","signStatus","type","radio","id","signStatus1",1,"form-check-input",3,"value","ngModel","checked","ngModelChange"],["for","signStatus1",1,"form-check-label"],["name","signStatus","type","radio","id","signStatus2",1,"form-check-input",3,"value","ngModel","checked","ngModelChange"],["for","signStatus2",1,"form-check-label"],["class","showDiv",4,"ngIf"],[1,""],["data-phlp","tooltip",1,"fa","fa-question-circle","pos-abs","hlpICNKey",2,"top","10px","right","-15px","cursor","pointer"],[1,"p-2","overflow-auto",2,"height","240px"],[1,"table","table-bordered"],["scope","col"],[4,"ngFor","ngForOf"],[1,"col-md-4","col-lg-2"],[1,"col-md-8","col-lg-10"],["name","myckeditor","required","","debounce","500",3,"ngModel","config","ngModelChange"],["myckeditor","ngModel"],[4,"ngIf","ngIfElse"],["update",""],[3,"value"],[1,"showDiv"],[1,"btn","btn-primary",3,"click"],[1,"btn","btn-danger","ml-1",3,"click"]],template:function(n,i){if(1&n&&(t.TgZ(0,"div",0)(1,"h4"),t._uU(2),t.qZA()(),t.TgZ(3,"div",1)(4,"div",2)(5,"div",3)(6,"ul",4)(7,"li",5)(8,"a",6),t.NdJ("click",function(){return i.gotoadd("0:0:"+i.linkId)}),t._uU(9),t.qZA()(),t.TgZ(10,"li",5)(11,"a",7),t.NdJ("click",function(){return i.gotoView("0:0:"+i.linkId)}),t._uU(12),t.qZA()()()(),t.TgZ(13,"div",8)(14,"div",9)(15,"div",10)(16,"div",11)(17,"div",12)(18,"div",10)(19,"label",13),t._uU(20),t.qZA(),t.TgZ(21,"div",14)(22,"select",15),t.NdJ("ngModelChange",function(r){return i.selFormName=r}),t.TgZ(23,"option",16),t._uU(24,"--Select--"),t.qZA(),t.YNc(25,M,2,2,"option",17),t.qZA()()()(),t.TgZ(26,"div",12)(27,"div",10)(28,"label",13),t._uU(29),t.qZA(),t.TgZ(30,"div",14)(31,"input",18),t.NdJ("ngModelChange",function(r){return i.txtLetterName=r})("keypress",function(r){return i.vldChkLst.isCharKey(r)})("keyup",function(r){return i.vldChkLst.blockspecialchar_first(r)}),t.qZA()()()(),t.TgZ(32,"div",12)(33,"div",10)(34,"label",13),t._uU(35),t.qZA(),t.TgZ(36,"div",14)(37,"select",19),t.NdJ("ngModelChange",function(r){return i.letterType=r}),t.TgZ(38,"option",20),t._uU(39,"--Select--"),t.qZA(),t.YNc(40,D,2,2,"option",21),t.qZA()()()(),t.TgZ(41,"div",12)(42,"div",10)(43,"label",13),t._uU(44),t.qZA(),t.TgZ(45,"div",14)(46,"div",22)(47,"input",23),t.NdJ("ngModelChange",function(r){return i.signStatus=r}),t.qZA(),t.TgZ(48,"label",24),t._uU(49),t.qZA()(),t.TgZ(50,"div",22)(51,"input",25),t.NdJ("ngModelChange",function(r){return i.signStatus=r}),t.qZA(),t.TgZ(52,"label",26),t._uU(53),t.qZA()()()()(),t.YNc(54,J,10,3,"div",27),t.qZA(),t.TgZ(55,"div",11)(56,"div",28)(57,"h6"),t._uU(58),t.TgZ(59,"small"),t._UZ(60,"i",29),t.qZA()(),t.TgZ(61,"div",30)(62,"table",31)(63,"thead")(64,"tr")(65,"th",32),t._uU(66),t.qZA(),t.TgZ(67,"th",32),t._uU(68),t.qZA()()(),t.TgZ(69,"tbody"),t.YNc(70,R,5,2,"tr",33),t.qZA()()()()()(),t.TgZ(71,"div",12)(72,"div",10)(73,"label",34),t._uU(74),t.qZA(),t.TgZ(75,"div",35)(76,"ckeditor",36,37),t.NdJ("ngModelChange",function(r){return i.txtLetterContent=r}),t.qZA()()()()(),t.TgZ(78,"div",12)(79,"div",10),t._UZ(80,"label",34),t.TgZ(81,"div",35),t.YNc(82,P,5,2,"ng-container",38),t.YNc(83,H,4,2,"ng-template",null,39,t.W1O),t.qZA()()()()()()),2&n){const s=t.MAs(84);t.xp6(2),t.Oqu(i.title),t.xp6(7),t.Oqu(i.itemID>0?"Edit":"Add"),t.xp6(3),t.Oqu("View"),t.xp6(8),t.hij("* ","Form Name",""),t.xp6(2),t.Q6J("ngModel",i.selFormName),t.xp6(3),t.Q6J("ngForOf",i.allFormsArr),t.xp6(4),t.hij("*","Letter Name"," "),t.xp6(2),t.Q6J("ngModel",i.txtLetterName),t.xp6(4),t.hij("*","Letter Type"," "),t.xp6(2),t.Q6J("ngModel",i.letterType),t.xp6(3),t.Q6J("ngForOf",i.letterTypeList),t.xp6(4),t.hij("*","Digital Sign Applicable"," "),t.xp6(3),t.Q6J("value",1)("ngModel",i.signStatus)("checked",!i.isSelected),t.xp6(2),t.Oqu("Yes"),t.xp6(2),t.Q6J("value",0)("ngModel",i.signStatus)("checked",i.isSelected),t.xp6(2),t.Oqu("No"),t.xp6(1),t.Q6J("ngIf",1==i.signStatus),t.xp6(4),t.hij("","Keys for value"," "),t.xp6(8),t.Oqu("Key"),t.xp6(2),t.Oqu("Label Name"),t.xp6(2),t.Q6J("ngForOf",i.keysArray),t.xp6(4),t.hij("*","Letter Content"," "),t.xp6(2),t.Q6J("ngModel",i.txtLetterContent)("config",i.ckeConfig),t.xp6(6),t.Q6J("ngIf",""==i.itemID||"0"==i.itemID)("ngIfElse",s)}},dependencies:[g.YN,g.Kr,g.Fj,g.EJ,g._,g.JJ,g.Q7,g.nD,g.On,_.sg,_.O5,O.u]})}return o})();var K=c(88663),Y=c(6593),x=c(76472);function j(o,d){if(1&o&&(t.TgZ(0,"option",24),t._uU(1),t.qZA()),2&o){const e=d.$implicit;t.Q6J("value",e.intId),t.xp6(1),t.hij(" ",e.vchProcessName," ")}}function Q(o,d){if(1&o){const e=t.EpF();t.TgZ(0,"tr")(1,"td"),t._uU(2),t.qZA(),t.TgZ(3,"td"),t._uU(4),t.qZA(),t.TgZ(5,"td"),t._uU(6),t.qZA(),t.TgZ(7,"td"),t._uU(8),t.qZA(),t.TgZ(9,"td",34)(10,"a",35),t._uU(11),t.qZA(),t.TgZ(12,"div",36)(13,"div",37)(14,"div",38)(15,"div",39)(16,"h5",40),t._uU(17),t.qZA(),t._UZ(18,"button",41),t.qZA(),t.TgZ(19,"div",42)(20,"div",43)(21,"table",44)(22,"tbody")(23,"tr")(24,"th",45),t._uU(25),t.qZA(),t.TgZ(26,"td",45),t._uU(27),t.qZA()(),t.TgZ(28,"tr")(29,"th",45),t._uU(30),t.qZA(),t.TgZ(31,"td",46),t._UZ(32,"div",47),t.qZA()()()()()()()()()(),t.TgZ(33,"td",34)(34,"a",48),t.NdJ("click",function(){const s=t.CHM(e).$implicit,r=t.oxw(2);return t.KtG(r.editLetter(s.intLetterConfigId+":"+s.intformId))}),t._UZ(35,"i",49),t.qZA(),t.TgZ(36,"a",50),t.NdJ("click",function(){const s=t.CHM(e).$implicit,r=t.oxw(2);return t.KtG(r.deleteLetter(s.intLetterConfigId))}),t._UZ(37,"i",51),t.qZA()()()}if(2&o){const e=d.$implicit,n=d.index,i=t.oxw(2);t.xp6(2),t.hij("",n+1+i.indexNumber," "),t.xp6(2),t.Oqu(e.vchFormName),t.xp6(2),t.Oqu(e.vchLetterName),t.xp6(2),t.Oqu(e.dtmCreatedOn),t.xp6(2),t.uIk("data-bs-target","#previewModal"+n),t.xp6(1),t.hij(" ","Preview"," "),t.xp6(1),t.MGl("id","previewModal",n,""),t.xp6(5),t.hij("","Letter Preview"," "),t.xp6(8),t.Oqu("Letter Name"),t.xp6(2),t.hij(" ",e.vchLetterName,""),t.xp6(3),t.hij("","Letter Content"," "),t.xp6(2),t.Q6J("innerHTML",i.sanitizer.bypassSecurityTrustHtml(e.txtLetterContent),t.oJD)}}const V=function(o,d,e){return{itemsPerPage:o,currentPage:d,totalItems:e}};function B(o,d){if(1&o){const e=t.EpF();t.TgZ(0,"div")(1,"div",25)(2,"table",26)(3,"thead")(4,"tr")(5,"th",27),t._uU(6),t.qZA(),t.TgZ(7,"th",28),t._uU(8),t.qZA(),t.TgZ(9,"th",28),t._uU(10),t.qZA(),t.TgZ(11,"th",28),t._uU(12),t.qZA(),t.TgZ(13,"th",29),t._uU(14),t.qZA(),t.TgZ(15,"th",30),t._uU(16),t.qZA()()(),t.TgZ(17,"tbody"),t.YNc(18,Q,38,12,"tr",31),t.ALo(19,"paginate"),t.qZA()()(),t.TgZ(20,"div",32)(21,"pagination-controls",33),t.NdJ("pageChange",function(i){t.CHM(e);const s=t.oxw();return t.KtG(s.onTableDataChange(i))}),t.qZA()()()}if(2&o){const e=t.oxw();t.xp6(6),t.hij("","Sl","#"),t.xp6(2),t.Oqu("Form Name"),t.xp6(2),t.Oqu("Letter Name"),t.xp6(2),t.Oqu("Created On"),t.xp6(2),t.Oqu("View"),t.xp6(2),t.hij(" ","Action",""),t.xp6(2),t.Q6J("ngForOf",t.xi3(19,7,e.letterlist,t.kEZ(10,V,e.tableSize,e.page,e.count)))}}function W(o,d){1&o&&(t.TgZ(0,"h6",52),t._uU(1),t.qZA()),2&o&&(t.xp6(1),t.hij(" ","No Record Found",""))}function z(o,d){1&o&&(t.TgZ(0,"div",53),t._UZ(1,"div",54),t.TgZ(2,"p"),t._uU(3),t.qZA()()),2&o&&(t.xp6(3),t.hij("","Loading","..."))}let F=(()=>{class o{constructor(e,n,i,s,r,l,p,u,T,v){this.fb=e,this.route=n,this.router=i,this.commonService=s,this.vldChkLst=r,this.encDec=l,this.workflowService=p,this.translate=u,this.LetterconfigService=T,this.sanitizer=v,this.title="View Letter Configuration",this.txtLetterName=null,this.selFormName=0,this.isFlag=!0,this.loading=!1,this.page=1,this.count=0,this.tableSize=10,this.pageSizes=[10,20,50,100,500,1e3],this.letterIdArray=[],this.pubUnpStatus=[],this.chkAll=0,this.sevName="letterconfig",this.langKey="en",this.indexNumber=0,this.intDeleteRight=0,this.intEditRight=0,this.intViewManageRight=0,this.intaddRight=0,this.intallRight=0,this.publishRight=0,this.processId="C2",this.allFormsArr=[],this.userid=0,this.createId=0,this.previllage=3,this.username=""}ngOnInit(){this.minDate=new Date,this.maxDate=new Date;let e=this.router.snapshot.paramMap.get("id");if(""!=e){let i=this.encDec.decText(e).split(":");this.itemID=i[0],this.resubmitstatus=i[1],this.linkId=i[2]}this.viewItems(),this.getAllFormName()}gotoView(e){this.encDec.encText(e.toString()),this.route.navigate(["/admin/letterconfiguration/viewLetterConfig"])}gotoadd(e){this.encDec.encText(e.toString()),this.route.navigate(["/admin/letterconfiguration/addLetterConfig"])}getAllFormName(){this.LetterconfigService.getAllForms([]).subscribe(n=>{let r=m.lW.from(n.RESPONSE_DATA,"base64");r=JSON.parse(r.toString()),200==r.status&&(this.allFormsArr=r.result)})}viewItems(){this.letterIdArray=[],this.selFormName=0,this.txtLetterName=null,this.loading=!0,this.pubUnpStatus=[],this.LetterconfigService.viewLetters({formId:"",LetterName:"",intLetterConfigId:""}).subscribe(n=>{let i=n.RESPONSE_DATA;if(n.RESPONSE_TOKEN==f.HmacSHA256(i,h.N.apiHashingKey).toString()){let l=m.lW.from(i,"base64");l=JSON.parse(l.toString()),200==l.status?(this.letterlist=l.result,this.count=l.totalCount,this.isFlag=!0,this.loading=!1):417==l.status?(this.isFlag=!1,a().fire({icon:"error",text:"invalid Response"})):(this.isFlag=!1,a().fire({icon:"error",text:"error Message"}))}else a().fire({icon:"error",text:"invalid Response"})})}viewSearchList(){let i={formId:this.selFormName,LetterName:this.txtLetterName,intLetterConfigId:""};this.loading=!0,this.LetterconfigService.viewLetters(i).subscribe(s=>{let r=s.RESPONSE_DATA;if(s.RESPONSE_TOKEN==f.HmacSHA256(r,h.N.apiHashingKey).toString()){let u=m.lW.from(r,"base64");u=JSON.parse(u.toString()),200==u.status?(this.letterlist=u.result,this.isFlag=!0,this.loading=!1):417==u.status?(this.isFlag=!1,a().fire({icon:"error",text:"invalid Response"})):(this.isFlag=!1,a().fire({icon:"error",text:"error Msg"}))}else this.isFlag=!1,a().fire({icon:"error",text:"invalid Response"})})}onChange(e,n,i){if(n.target.checked)this.letterIdArray.includes(e)||(this.letterIdArray.push(e),this.pubUnpStatus.push({letterId:e,publishUnpublisStatus:i}));else{let s=this.letterIdArray.indexOf(e),r=0;for(let l of this.pubUnpStatus){if(l.letterId==e){this.pubUnpStatus.splice(r,1);break}r++}this.letterIdArray.splice(s,1)}}onTableDataChange(e){this.page=e}onTableSizeChange(e){this.tableSize=e.target.value,this.page=1}editLetter(e){let n=this.encDec.encText(e.toString());this.route.navigate(["/admin/letterconfiguration/editLetterConfig",n])}deleteLetter(e){let n={itemId:e,itemStatus:"1"};a().fire({title:"Are you sure?",text:"You Want to Delete This Record!",icon:"warning",showCancelButton:!0,cancelButtonText:"Cancel",confirmButtonColor:"#3085d6",cancelButtonColor:"#d33",confirmButtonText:"Yes, delete it!"}).then(i=>{i.isConfirmed&&this.LetterconfigService.deleteLetters(n).subscribe(s=>{let r=s.RESPONSE_DATA;if(s.RESPONSE_TOKEN==f.HmacSHA256(r,h.N.apiHashingKey).toString()){let u=m.lW.from(r,"base64");u=JSON.parse(u.toString()),200==u.status?(a().fire({text:"Record Deleted Successfully!",icon:"success",confirmButtonColor:"#3085d6",confirmButtonText:"Ok"}),this.viewItems()):417==u.status?a().fire({text:"Invalid Response !",icon:"warning"}):a().fire({text:"something went wrong",icon:"error"})}else a().fire({text:"Invalid Response",icon:"error"})})})}getHtml(e){return this.sanitizer.bypassSecurityTrustHtml(e)}static#t=this.\u0275fac=function(n){return new(n||o)(t.Y36(g.qu),t.Y36(S.F0),t.Y36(S.gz),t.Y36(L.R),t.Y36(b._),t.Y36(I.q),t.Y36(K.z),t.Y36(k.sK),t.Y36(w.D),t.Y36(Y.H7))};static#e=this.\u0275cmp=t.Xpm({type:o,selectors:[["app-viewletterconfiguration"]],decls:37,vars:11,consts:[[1,"page-title"],["id","page-content",1,"page-controls-section"],[1,"card"],[1,"controls-section-header"],[1,"nav","nav-tabs"],[1,"nav-item"],["aria-current","page",1,"nav-link",3,"click"],[1,"nav-link","active",3,"click"],[1,"card-body"],[1,"controls-section"],[1,"search-container","active"],[1,"search-sec"],[1,"row"],[1,"col-12","col-md-3","col-lg-3"],[1,"form-group"],["id","selFormName",1,"form-select",3,"ngModel","ngModelChange"],["value","0"],[3,"value",4,"ngFor","ngForOf"],["type","text","placeholder","Letter Name",1,"form-control",3,"ngModel","ngModelChange"],[1,"btn","btn-primary",3,"click"],[1,"btn","btn-danger","ml-1",3,"click"],[4,"ngIf","ngIfElse"],["norecord",""],["class","loader",4,"ngIf"],[3,"value"],[1,"table-responsive","print-section"],["data-toggle","table",1,"table","table-bordered","valign-middle"],["scope","col",2,"width","40px"],["scope","col"],["scope","col",1,"noPrint"],["scope","col",1,"noPrint",2,"width","80px"],[4,"ngFor","ngForOf"],[1,"d-flex","justify-content-end"],["previousLabel","Prev","nextLabel","Next",3,"pageChange"],[1,"noPrint"],["type","button","data-bs-toggle","modal",1,"text-info"],["tabindex","-1","aria-labelledby","previewModalLabel","aria-hidden","true",1,"modal","fade",3,"id"],[1,"modal-dialog","modal-lg"],[1,"modal-content"],[1,"modal-header"],["id","previewModalLabel",1,"modal-title","mb-0"],["type","button","data-bs-dismiss","modal","aria-label","Close",1,"btn-close"],[1,"modal-body"],[1,"table-responsive"],[1,"table","table-bordered"],[2,"width","30%"],[2,"width","70%"],[1,"pdfContainer",3,"innerHTML"],["data-toggle","tooltip","title","","data-original-title","Edit",1,"text-primary",3,"click"],[1,"bi","bi-pencil-square"],["data-toggle","tooltip","title","","data-original-title","Delete",1,"text-danger",3,"click"],[1,"bi","bi-trash3"],[1,"no-content"],[1,"loader"],[1,"loader-item"]],template:function(n,i){if(1&n&&(t.TgZ(0,"div",0)(1,"h4"),t._uU(2),t.qZA()(),t.TgZ(3,"div",1)(4,"div",2)(5,"div",3)(6,"ul",4)(7,"li",5)(8,"a",6),t.NdJ("click",function(){return i.gotoadd("0:0:"+i.linkId)}),t._uU(9),t.qZA()(),t.TgZ(10,"li",5)(11,"a",7),t.NdJ("click",function(){return i.gotoView("0:0:"+i.linkId)}),t._uU(12),t.qZA()()()(),t.TgZ(13,"div",8)(14,"div",9)(15,"div",10)(16,"div",11)(17,"div",12)(18,"div",13)(19,"div",14)(20,"select",15),t.NdJ("ngModelChange",function(r){return i.selFormName=r}),t.TgZ(21,"option",16),t._uU(22,"--Select--"),t.qZA(),t.YNc(23,j,2,2,"option",17),t.qZA()()(),t.TgZ(24,"div",13)(25,"div",14)(26,"input",18),t.NdJ("ngModelChange",function(r){return i.txtLetterName=r}),t.qZA()()(),t.TgZ(27,"div",13)(28,"div",14)(29,"button",19),t.NdJ("click",function(){return i.viewSearchList()}),t._uU(30),t.qZA(),t.TgZ(31,"button",20),t.NdJ("click",function(){return i.viewItems()}),t._uU(32),t.qZA()()()()()(),t.YNc(33,B,22,14,"div",21),t.YNc(34,W,2,1,"ng-template",null,22,t.W1O),t.qZA()()()(),t.YNc(36,z,4,1,"div",23)),2&n){const s=t.MAs(35);t.xp6(2),t.Oqu(i.title),t.xp6(7),t.Oqu("Add"),t.xp6(3),t.Oqu("View"),t.xp6(8),t.Q6J("ngModel",i.selFormName),t.xp6(3),t.Q6J("ngForOf",i.allFormsArr),t.xp6(3),t.Q6J("ngModel",i.txtLetterName),t.xp6(4),t.hij(" ","Show",""),t.xp6(2),t.hij(" ","Reset",""),t.xp6(1),t.Q6J("ngIf",(null==i.letterlist?null:i.letterlist.length)>0)("ngIfElse",s),t.xp6(3),t.Q6J("ngIf",i.loading)}},dependencies:[g.YN,g.Kr,g.Fj,g.EJ,g.JJ,g.On,x.LS,_.sg,_.O5,x._s]})}return o})();var A=c(57622);const G=[{path:"",component:Z},{path:"",component:Z,children:[{path:"addLetterConfig",component:N,canActivate:[A.a]},{path:"addLetterConfig/:id",component:N,canActivate:[A.a]},{path:"editLetterConfig/:id",component:N,canActivate:[A.a]},{path:"viewLetterConfig",component:F,canActivate:[A.a]},{path:"viewLetterConfig/:id",component:F,canActivate:[A.a]}]}];let $=(()=>{class o{static#t=this.\u0275fac=function(n){return new(n||o)};static#e=this.\u0275mod=t.oAB({type:o});static#i=this.\u0275inj=t.cJS({imports:[S.Bz.forChild(G),S.Bz]})}return o})();var X=c(69862),tt=c(61420),et=c(60420),it=c(2639);let nt=(()=>{class o{static#t=this.\u0275fac=function(n){return new(n||o)};static#e=this.\u0275mod=t.oAB({type:o});static#i=this.\u0275inj=t.cJS({imports:[X.JF,g.u5,g.UX,x.JX,tt.IJ,et.UM,it.kn.forRoot(),_.ez,O.d,_.ez,$]})}return o})()}}]);