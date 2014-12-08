/** @jsx React.DOM */

(function () {

    var dataSource = {};

    /*jshint ignore:start */
    var TreeNode = React.createClass({
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
            if (this.props.node.contents != null) {
                childNodes = this.props.node.contents.map(function (node, index) {
                    return <li key={index} className={node.type}>
                        <TreeNode node={node} />
                    </li>
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

            return (
                <div>
                    <h5 onClick={this.toggle} className={className}>
          {this.props.node.description}
                    </h5>
                    <ul style={style}>
          {childNodes}
                    </ul>
                </div>
            );
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
        setLoading(document.getElementById('allocatorShareTree'))
        var managerSelectList = document.getElementById('CategoryManagerList')
        managerSelectList.innerHTML = '';

        contentsXhr = $.getJSON("/api/share/" + id, function (data) {
            dataSource = data.allocatorList;
            React.render(
                <TreeNode node={dataSource} />,
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

    $("#ApplyDepartment").on("click", function(){
        var value = $("#ddlDept").val();
        getContents(value);
        //getManagers(value);
    });


})();
/*jshint ignore:end */