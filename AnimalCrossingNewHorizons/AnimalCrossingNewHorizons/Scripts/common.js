$(function () {

    // ログアウトリンククリック処理
    $('#LogoutLink').click(function () {
        $("#dialogConfirm").dialog({
            width: 400,       // 横幅のサイズを設定
            modal: true,      // モーダルダイアログにする
            buttons: [        // ボタン名 : 処理 を設定
                {
                    text: 'OK',
                    click: function () {
                        $.ajax({
                            type: 'POST',
                            url: "/Auth/Logout",
                            contentType: 'application/json',
                            dataType: "json",
                            success: function (data, status, xhr) {
                                if (xhr.status === 200) {
                                    location.href = "/";
                                } else {
                                    alert("問題が発生しました。");
                                }
                            }
                        });
                    }
                },
                {
                    text: 'キャンセル',
                    click: function () {
                        // ダイアログを閉じる
                        $(this).dialog("close");
                    }
                }
            ]
        });
    });
})