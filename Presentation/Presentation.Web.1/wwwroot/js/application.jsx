class CommentBox {
    //test: test;

    render() {
        return (
          <div>
              Hello, world! I am a CommentBox.
              <button>Click me!</button>
          </div>
        );
    }
};

ReactDOM.render(React.createElement(CommentBox, null), document.getElementById('content'));