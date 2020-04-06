using System.Globalization;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class RecAndPlay : UdonSharpBehaviour
{
    // 記録する周期(秒)
    private int _recordCycleSecond = 1;

    // 記録する時間(分)
    private int _recordMinutes = 1;

    // 時間表示
    [SerializeField] private Text _timeText;

    // オリジナル
    [SerializeField] private GameObject _originObject;

    // ゴースト
    [SerializeField] private GameObject _ghostObject;

    // 記録開始からの経過時間
    private float _leftTime = 0;

    // タイマー用 経過時間保持
    private float _timer = 0;

    // 移動座標保存用配列
    private Vector3[] _pathLog;

    // 移動時間を保存する配列
    private float[] _pathTime;

    private int _pathCount = 0;

    // 記録中か
    private bool _isRecording = false;

    // 再生中か
    private bool _isPlaying = false;

    private int _playPathCount = 0;

    void Start()
    {
        _timer = _recordCycleSecond;

        // 移動座標を保存する配列　記録周期(秒)を分になおして　RecordMinutes分記録できる想定
        _pathLog = new Vector3[_recordCycleSecond * (60 / _recordCycleSecond) * _recordMinutes];

        // 移動時間を保存する
        _pathTime = new float[_recordCycleSecond * (60 / _recordCycleSecond) * _recordMinutes];
    }

    private void Update()
    {
        if (_isRecording)
        {
            // 経過時間加算 出力
            _leftTime += Time.deltaTime;
            _timeText.text = "記録時間:" + _leftTime;

            // タイマー処理
            _timer += Time.deltaTime;

            // 周期ごとに実行
            if (_timer >= _recordCycleSecond)
            {
                _timer -= _recordCycleSecond; //初期化
                RecordPosition();
            }
        }

        if (_isPlaying)
        {
            // 次のpathに近づいたら移動先を更新?
            if (Vector3.Distance(_ghostObject.transform.position, _pathLog[_playPathCount]) < 0.2f)
            {
                _playPathCount++;
            }

            // 最初 開始位置に移動
            if (_playPathCount == 0)
            {
                _ghostObject.transform.position = _pathLog[_playPathCount];
                return;
            }

            // 最後 終了処理をして抜ける
            if (_pathCount == _playPathCount)
            {
                _isPlaying = false;
                return;
            }

            // 各点の 速さ = 距離 / 時間
            var speed = Vector3.Distance(_pathLog[_playPathCount],
                _pathLog[_playPathCount - 1] / (_pathTime[_playPathCount] - _pathTime[_playPathCount - 1]));

            // pathへ移動
            _ghostObject.transform.position = Vector3.MoveTowards(_ghostObject.transform.position,
                _pathLog[_playPathCount], speed * Time.deltaTime);

            _ghostObject.transform.LookAt(new Vector3(_pathLog[_playPathCount].x, _ghostObject.transform.position.y,
                _pathLog[_pathCount].z));
        }
    }

    private void RecordPosition()
    {
        _pathLog[_pathCount] = _originObject.transform.position;
        _pathTime[_pathCount] = _leftTime;
        _pathCount++;

        if (_pathCount >= _pathLog.Length - 1)
        {
            _isRecording = false;
            Debug.Log("タイムアップ");
        }
    }

    private void RecordInitialize()
    {
        // 記録開始
        _isRecording = true;

        // 初期化
        _timer = _recordCycleSecond;
        _leftTime = 0;
        _pathLog = new Vector3[_recordCycleSecond * (60 / _recordCycleSecond) * _recordMinutes];
        _pathTime = new float[_recordCycleSecond * (60 / _recordCycleSecond) * _recordMinutes];
        _pathCount = 0;
    }

    public void GhostRecButton()
    {
        if (_isRecording)
        {
            // 停止し、記録終了
            _isRecording = false;
        }
        else
        {
            RecordInitialize();
        }
    }

    public void GhostPlayButton()
    {
        // 記録中なら抜ける
        if (_isRecording) return;
        // 記録がなくても抜ける
        //if(_playPathCount == 0) return;

        // 開始処理
        _isPlaying = true;

        // 初期化
        _playPathCount = 0;
    }
}