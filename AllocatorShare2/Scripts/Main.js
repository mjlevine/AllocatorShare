/** @jsx React.DOM */

(function () {

    var dataSource = {};

    /*jshint ignore:start */
    var TreeNode = React.createClass({
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
          {this.props.node.name}
                    </h5>
                    <ul style={style}>
          {childNodes}
                    </ul>
                </div>
            );
        }

    });




    var getContents = function (id) {
        document.getElementById('allocatorShareTree').innerHTML = '';
        $.getJSON("/api/share/" + id, function (data) {
            dataSource = data;
            React.render(
                <TreeNode node={dataSource} />,
                document.getElementById('allocatorShareTree')
            );
        });
    }

    $.getJSON("/api/rootlist/1", function (data) {
        var listItems = '';
        _.each(data, function(item){
            listItems+= "<option value='" + item.value + "'>" + item.text + "</option>";
        });
        $("#ddlDept").html(listItems);

        getContents(data[0].value);
    });

    $("#ApplyDepartment").on("click", function(){
        var value = $("#ddlDept").val();
        getContents(value);
    });


})();
/*jshint ignore:end */