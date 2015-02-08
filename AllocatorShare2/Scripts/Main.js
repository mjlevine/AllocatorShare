/** @jsx React.DOM */

(function () {

    var dataSource = {};

    var TreeNode = React.createClass({displayName: 'TreeNode',
        xhr: false,
        getInitialState: function () {
            return {
                visible: true
            };
        },
        collapseAll: function () {
            this.setState({
                visible: this.state.visible.map(function () {
                    return true;
                })
            });
        },
        toggle: function () {
            this.setState({visible: !this.state.visible});
        },
        render: function () {
            var childNodes;
            var className = "";
            if (this.props.node.contents !== null) {
                childNodes = this.props.node.contents.map(function (node, index) {
                    return React.createElement("li", {key: index, className: node.type}, 
                        React.createElement(TreeNode, {node: node})
                    );
                });

                className = this.props.node.contents == "folder" ? "togglable" : "";
                if (this.state.visible) {
                    className += " togglable-down";
                } else {
                    className += " togglable-up";
                }
            }

            var style = {};
            if (!this.state.visible) {
                style.display = "none";
            }

            if (this.props.node.type === "file") {
                return React.createElement("h5", null, 
                    React.createElement("a", {href: this.props.node.downloadUrl}, this.props.node.description)
                );
            } else {
                return (
                    React.createElement("div", null, 
                        React.createElement("h5", {onClick: this.toggle, className: className}, 
          this.props.node.description
                        ), 
                        React.createElement("ul", {style: style}, 
          childNodes
                        )
                    )
                );
            }
        }

    });

    var setLoading = function(el) {
        el.innerHTML = '';
        var loading = document.createElement("div");
        loading.className = "loading";
        el.appendChild(loading);
    };

    var contentsXhr, managersXhr;
    var getContents = function (id) {
        if(contentsXhr) {
            contentsXhr.abort();
        }
        setLoading(document.getElementById('ManagerLoading'));
        setLoading(document.getElementById('allocatorShareTree'));
        var managerSelectList = document.getElementById('CategoryManagerList');
        managerSelectList.innerHTML = '';

        contentsXhr = $.getJSON("/api/share/" + id, function (data) {
            dataSource = data.allocatorList;
            React.render(
                React.createElement(TreeNode, {node: dataSource}),
                document.getElementById('allocatorShareTree')
            );


            var listItems = '';
            _.each(data.managerList, function(item){
                listItems+= "<option value='" + item.value + "'>" + item.text + "</option>";
            });
            managerSelectList.innerHTML = listItems;
            document.getElementById('ManagerLoading').innerHTML = '';

        });
    };



    setLoading(document.getElementById('DepartmentLoading'));
    $.getJSON("/api/rootlist/1", function (data) {
        var listItems = '';
        _.each(data, function(item){
            listItems+= "<option value='" + item.value + "'>" + item.text + "</option>";
        });
        $("#ddlDept").html(listItems);
        document.getElementById('DepartmentLoading').innerHTML = '';
        getContents(data[0].value);
        //getManagers(data[0].value);
    });

    $("#ddlDept").on("change", function(){
        var value = $("#ddlDept").val();
        getContents(value);
    });


    $("#imbtnUpload1").on("click", function(){
       $("#selectionContainer").hide();
	   $("#uploadSubmit").removeAttr("disabled")
        $("#uploadContainer").show();
        $("#targetFolderLabel").html($("#CategoryManagerList option:selected").text())
    });

    $("#imbtnSelect1").on("click", function(){
        $('#fileUploadField').val('');
        $(".responseMessage").html("");

        $("#selectionContainer").show();
        $("#uploadContainer").hide();
    });

    $("#uploadSubmit").on("click", function() {
        var $button = $(this),
            value = $("#CategoryManagerList").val(),
            formData,
            $responseMessage = $(".responseMessage");

        $("#uploadRootFolderId").val(value);
        $button.attr("disabled","disabled");
        $responseMessage.html("");

        formData = new FormData(document.getElementById("fileUploadForm"));
        $.ajax({
            url: '/api/upload/' + value,
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (returndata) {
                $responseMessage.html("File Uploaded - You may upload another file.");
                $button.removeAttr("disabled")
                $('#fileUploadField').val('');
            },
            error: function(err) {
                $responseMessage.html($("<div>").addClass("error").text(err.responseJSON.message));
                $button.removeAttr("disabled")
            }
        });

        return false;
    });
})();