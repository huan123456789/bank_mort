<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="bankMort_login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8">
    <title>按揭系统登录</title>
    <link rel="shortcut icon" type="image/x-icon" href="http://m.hshb.com/images/hshb16.ico"
        media="screen">
    <meta name="renderer" content="webkit">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">
    <script src="js/jquery-1.8.3.min.js" type="text/javascript"></script>
    <script src="js/comm/jquery-ajax.js" type="text/javascript"></script>
    <script src="js/bank/validate.min.js" type="text/javascript"></script>
    <script src="js/bank/formcheck.js" type="text/javascript"></script>
    <script src="js/bank/commclient.js" type="text/javascript"></script>
    <link href="js/layui/css/layui.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form runat="server" class="layui-form">
    <div class="layui-header layui-layout layui-layout-admin" style="width: 300px; height: 220px;
        position: absolute; left: 50%; top: 30%; margin-left: -200px;" lay-filter="divlogin">
        <div class="layui-form-item">
            <label class="layui-form-label" style=" width:50px;">
                银行</label>
            <div class="layui-input-block" style=" width:195px;">
                <select id="s_banklist" runat="server" class="layui-input" name="pay_type" style="width: 80px;">
                </select>
            </div>
        </div>
        <div class="layui-form-item">
            <label class="layui-form-label" style=" width:50px;">
                账号</label>
            <div class="layui-input-block" style=" width:195px;">
                <input id="txt_username" runat="server" type="text" class="layui-input" name="title"
                    placeholder="请输入账号" lay-verify="title" autocomplete="off" />
            </div>
        </div>
        <div class="layui-form-item">
            <label class="layui-form-label" style=" width:50px;">
                密码</label>
            <div class="layui-input-block" style=" width:195px;">
                <input id="txt_pwd" type="password" runat="server" name="title" lay-verify="title"
                    autocomplete="off" placeholder="请输入密码" class="layui-input">
            </div>
        </div>
        <div class="layui-form-item" style="text-align: center;">
            <asp:Button ID="bnt_login" Text="登陆" CssClass="layui-btn" OnClientClick="saveloginbank();"
                runat="server" OnClick="bnt_login_Click" />
        </div>
    </div>
    <input runat="server" type="hidden" id="hf_bankname" />
    <script src="js/layui/layui.all.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        layui.use('form', function () {
            var form = layui.form;
            function loadbanklist() {
                var kv = {
                    't': 101
                };
                asyncAjax(publicApi.apiList.bankurl, kv, loadbanklistsuccess);

                function loadbanklistsuccess(model) {
                    var typehtml = '<option value="">--选择--</option>';
                    if (model.code == 0) {
                        var data = model.data;
                        for (var i in data) {
                            typehtml += "<option value='" + data[i].value + "'>" + data[i].value + "</option>";
                        }
                        $("#s_banklist").html('').append(typehtml);
                        if (publicApi.getCache('bankvalue') != null)
                            $("#s_banklist").val(publicApi.getCache('bankvalue'));
                        form.render('select');
                    } else {
                        layer.msg(model.msg);
                    }
                }
            }
            loadbanklist();
        });
        function saveloginbank() {
            var bvalue = $('#s_banklist').val();
            $("#hf_bankname").val(bvalue);
            publicApi.setCache('bankvalue', bvalue);
        }
    </script>
    </form>
</body>
</html>
