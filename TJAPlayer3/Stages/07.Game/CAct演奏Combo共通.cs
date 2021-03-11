﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using SlimDX;
using FDK;

namespace TJAPlayer3
{
	internal class CAct演奏Combo共通 : CActivity
	{
		// プロパティ

		public STCOMBO n現在のコンボ数;
		public struct STCOMBO
		{
			public CAct演奏Combo共通 act;

			public int this[ int index ]
			{
				get
				{
					switch( index )
					{
						case 0:
							return this.P1;

						case 1:
							return this.P2;

						case 2:
							return this.P3;

                        case 3:
                            return this.P4;
					}
					throw new IndexOutOfRangeException();
				}
				set
				{
					switch( index )
					{
						case 0:
							this.P1 = value;
							return;

						case 1:
							this.P2 = value;
							return;

						case 2:
							this.P3 = value;
							return;

						case 3:
							this.P4 = value;
							return;
					}
					throw new IndexOutOfRangeException();
				}
			}
			public int P1
			{
				get
				{
					return this.p1;
				}
				set
				{
					this.p1 = value;
					if( this.p1 > this.P1最高値 )
					{
						this.P1最高値 = this.p1;
					}
					this.act.status.P1.nCOMBO値 = this.p1;
					this.act.status.P1.n最高COMBO値 = this.P1最高値;
				}
			}
			public int P2
			{
				get
				{
					return this.p2;
				}
				set
				{
					this.p2 = value;
					if( this.p2 > this.P2最高値 )
					{
						this.P2最高値 = this.p2;
					}
					this.act.status.P2.nCOMBO値 = this.p2;
					this.act.status.P2.n最高COMBO値 = this.P2最高値;
				}
			}
			public int P3
			{
				get
				{
					return this.p3;
				}
				set
				{
					this.p3 = value;
					if( this.p3 > this.P3最高値 )
					{
						this.P3最高値 = this.p3;
					}
					this.act.status.P3.nCOMBO値 = this.p3;
					this.act.status.P3.n最高COMBO値 = this.P3最高値;
				}
			}
			public int P4
			{
				get
				{
					return this.p4;
				}
				set
				{
					this.p4 = value;
					if( this.p4 > this.P4最高値 )
					{
						this.P4最高値 = this.p4;
					}
					this.act.status.P4.nCOMBO値 = this.p4;
					this.act.status.P4.n最高COMBO値 = this.P4最高値;
				}
			}
			public int P1最高値 { get; private set; }
			public int P2最高値 { get; private set; }
			public int P3最高値 { get; private set; }
			public int P4最高値 { get; private set; }

			private int p1;
			private int p2;
			private int p3;
			private int p4;
		}
		public C演奏判定ライン座標共通 演奏判定ライン座標
		{
			get;
			set;
		}

		protected enum EEvent { 非表示, 数値更新, 同一数値, ミス通知 }
		protected enum EMode { 非表示中, 進行表示中, 残像表示中 }
		protected const int nドラムコンボのCOMBO文字の高さ = 32;
		protected const int nドラムコンボのCOMBO文字の幅 = 90;
		protected const int nドラムコンボの高さ = 115;
		protected const int nドラムコンボの幅 = 90;
		protected const int nドラムコンボの文字間隔 = -6;
		protected int[] nジャンプ差分値 = new int[ 180 ];
		protected CSTATUS status;
        //protected CTexture txCOMBO太鼓;
        //protected CTexture txCOMBO太鼓_でかいやつ;
        //protected CTexture txコンボラメ;
        public CCounter[] ctコンボ加算;
        public CCounter ctコンボラメ;

        protected float[,] nコンボ拡大率_座標 = new float[,]{
                        {1.11f,-7},
                        {1.22f,-14},
                        {1.2f,-12},
                        {1.15f,-9},
                        {1.13f,-8},
                        {1.11f,-7},
                        {1.06f,-3},
                        {1.04f,-2},
                        {1.0f,0},
                    };
        protected float[,] nコンボ拡大率_座標_100combo = new float[,]{
                        {0.81f,-7},
                        {0.92f,-14},
                        {0.9f,-12},
                        {0.85f,-9},
                        {0.83f,-8},
                        {0.81f,-7},
                        {0.78f,-3},
                        {0.74f,-2},
                        {0.7f,0},
                };
    protected float[,] nコンボ拡大率_座標_1000combo = new float[,]{
                        {1.11f,-7},
                        {1.22f,-14},
                        {1.2f,-12},
                        {1.15f,-9},
                        {1.13f,-8},
                        {1.11f,-7},
                        {1.06f,-3},
                        {1.04f,-2},
                        {1.0f,0},
                    };


        private float[] ComboScale = new float[]
		{
			 0.000f,
			 0.072f,
			 0.170f,
			 0.146f,
			 0.108f,
			 0.096f,
			 0.084f,
			 0.066f,
			 0.044f,
			 0.033f,
			 0.022f,
			 0.011f,
			 0.000f,
		};
		private float[,] ComboScale_Ex = new float[,]
		{
			{ 0.000f, 0},
			{ 0.072f, 0},
			{ 0.200f, 0},
			{ 0.160f, 0},
			{ 0.130f, 0},
			{ 0.100f, 0},
			{ 0.084f, 0},
			{ 0.076f, 0},
			{ 0.044f, 0},
			{ 0.033f, 0},
			{ 0.022f, 0},
			{ 0.011f, 0},
			{ 0.000f, 0}
		};
		// 内部クラス

		protected class CSTATUS
		{
			public CSTAT P1 = new CSTAT();
			public CSTAT P2 = new CSTAT();
			public CSTAT P3 = new CSTAT();
			public CSTAT P4 = new CSTAT();
			public CSTAT this[ int index ]
			{
				get
				{
					switch( index )
					{
						case 0:
							return this.P1;

						case 1:
                            return this.P2;

						case 2:
							return this.P3;

						case 3:
							return this.P4;
					}
					throw new IndexOutOfRangeException();
				}
				set
				{
					switch( index )
					{
						case 0:
							this.P1 = value;
							return;

							this.P2 = value;
							return;

						case 2:
							this.P3 = value;
							return;

						case 3:
							this.P4 = value;
							return;
					}
					throw new IndexOutOfRangeException();
				}
			}

			public class CSTAT
			{
				public CAct演奏Combo共通.EMode e現在のモード;
				public int nCOMBO値;
				public long nコンボが切れた時刻;
				public int nジャンプインデックス値;
				public int n現在表示中のCOMBO値;
				public int n最高COMBO値;
				public int n残像表示中のCOMBO値;
				public long n前回の時刻_ジャンプ用;
			}
		}


		// コンストラクタ

		public CAct演奏Combo共通()
		{
			this.b活性化してない = true;

			// 180度分のジャンプY座標差分を取得。(0度: 0 → 90度:-15 → 180度: 0)
			for( int i = 0; i < 180; i++ )
				this.nジャンプ差分値[ i ] = (int) ( -15.0 * Math.Sin( ( Math.PI * i ) / 180.0 ) );
			演奏判定ライン座標 = new C演奏判定ライン座標共通();
		}


		// メソッド

		protected virtual void tコンボ表示_ドラム( int nCombo値, int nジャンプインデックス )
		{
		}

      	protected virtual void tコンボ表示_太鼓( int nCombo値, int nジャンプインデックス, int nPlayer )
		{
            //テスト用コンボ数
            //nCombo値 = 114;
			#region [ 事前チェック。]
			//-----------------
			//if( CDTXMania.ConfigIni.bドラムコンボ表示 == false )
			//	return;		// 表示OFF。

			if( nCombo値 == 0 )
				return;		// コンボゼロは表示しない。
			//-----------------
			#endregion

			int[] n位の数 = new int[ 10 ];	// 表示は10桁もあれば足りるだろう

            this.ctコンボラメ.t進行Loop();
            this.ctコンボ加算[ nPlayer ].t進行();

			#region [ nCombo値を桁数ごとに n位の数[] に格納する。（例：nCombo値=125 のとき n位の数 = { 5,2,1,0,0,0,0,0,0,0 } ） ]
			//-----------------
			int n = nCombo値;
			int n桁数 = 0;
			while( ( n > 0 ) && ( n桁数 < 10 ) )
			{
				n位の数[ n桁数 ] = n % 10;		// 1の位を格納
				n = ( n - ( n % 10 ) ) / 10;	// 右へシフト（例: 12345 → 1234 ）
				n桁数++;
			}
			//-----------------
			#endregion

			#region [ n位の数[] を、"COMBO" → 1の位 → 10の位 … の順に、右から左へ向かって順番に表示する。]
			//-----------------
			const int n1桁ごとのジャンプの遅れ = 30;	// 1桁につき 50 インデックス遅れる


            //X右座標を元にして、右座標 - ( コンボの幅 * 桁数 ) でX座標を求めていく?

			int nY上辺位置px = TJAPlayer3.ConfigIni.bReverse.Drums ? 350 : 10;
			int n数字とCOMBOを合わせた画像の全長px = ( ( 44 ) * n桁数 );
			int x = 245 + ( n数字とCOMBOを合わせた画像の全長px / 2 );
			//int y = 212;
            //int y = CDTXMania.Skin.nComboNumberY[ nPlayer ];

            #region[ コンボ文字 ]
            if( n桁数 <= 2 )
            {
                TJAPlayer3.Tx.Taiko_Combo_Text?.t2D拡大率考慮下中心基準描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Game_Taiko_Combo_Text_X[nPlayer], TJAPlayer3.Skin.Game_Taiko_Combo_Text_Y[nPlayer], new Rectangle(0, 0, TJAPlayer3.Skin.Game_Taiko_Combo_Text_Size[0], TJAPlayer3.Skin.Game_Taiko_Combo_Text_Size[1]));
            }
            else
            {
                TJAPlayer3.Tx.Taiko_Combo_Text?.t2D拡大率考慮下中心基準描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Game_Taiko_Combo_Text_X[nPlayer], TJAPlayer3.Skin.Game_Taiko_Combo_Text_Y[nPlayer], new Rectangle(0, TJAPlayer3.Skin.Game_Taiko_Combo_Text_Size[1], TJAPlayer3.Skin.Game_Taiko_Combo_Text_Size[0], TJAPlayer3.Skin.Game_Taiko_Combo_Text_Size[1]));
            }
            #endregion

            int rightX = 0;
            #region 一番右の数字の座標の決定
            if( n桁数 == 1)
            {
                // 一桁ならそのままSkinConfigの座標を使用する。
                rightX = TJAPlayer3.Skin.Game_Taiko_Combo_X[nPlayer];
            }
            else if( n桁数 == 2)
            {
                // 二桁ならSkinConfigの座標+パディング/2を使用する
                rightX = TJAPlayer3.Skin.Game_Taiko_Combo_X[nPlayer] + TJAPlayer3.Skin.Game_Taiko_Combo_Padding[0] / 2;
            }
            else if( n桁数 == 3)
            {
                // 三桁ならSkinConfigの座標+パディングを使用する
                rightX = TJAPlayer3.Skin.Game_Taiko_Combo_Ex_X[nPlayer] + TJAPlayer3.Skin.Game_Taiko_Combo_Padding[1];
            }
            else if (n桁数 == 4)
            {
                // 四桁ならSkinconfigの座標+パディング/2 + パディングを使用する
                rightX = TJAPlayer3.Skin.Game_Taiko_Combo_Ex4_X[nPlayer] + TJAPlayer3.Skin.Game_Taiko_Combo_Padding[2] / 2 + TJAPlayer3.Skin.Game_Taiko_Combo_Padding[2];
            }
            else
            {
                // 五桁以上の場合
                int rightDigit = 0;
                switch (n桁数 % 2)
                {
                    case 0:
                        // 2で割り切れる
                        // パディング/2を足す必要がある
                        // 右に表示される桁数を求め、-1する
                        rightDigit = n桁数 / 2 - 1;
                        rightX = TJAPlayer3.Skin.Game_Taiko_Combo_Ex4_X[nPlayer] + TJAPlayer3.Skin.Game_Taiko_Combo_Padding[2] / 2 + TJAPlayer3.Skin.Game_Taiko_Combo_Padding[2] * rightDigit;
                        break;
                    case 1:
                        // 2で割るとあまりが出る
                        // そのままパディングを足していく
                        // 右に表示される桁数を求める(中央除く -1)
                        rightDigit = (n桁数 - 1) /2;
                        rightX = TJAPlayer3.Skin.Game_Taiko_Combo_Ex4_X[nPlayer] + TJAPlayer3.Skin.Game_Taiko_Combo_Padding[2] * rightDigit;
                        break;
                    default:
                        break;
                }
            }
            #endregion


            for ( int i = 0; i < n桁数; i++ )
            {

                TJAPlayer3.Tx.Taiko_Combo[0].Opacity = 255;
                TJAPlayer3.Tx.Taiko_Combo[1].Opacity = 255;

                if ( n桁数 <= 1 )
                {
				    if(TJAPlayer3.Tx.Taiko_Combo[0] != null )
				    {
                        var yScalling = ComboScale[this.ctコンボ加算[nPlayer].n現在の値];
                        TJAPlayer3.Tx.Taiko_Combo[0].vc拡大縮小倍率.Y = TJAPlayer3.Skin.Game_Taiko_Combo_Scale[0] + yScalling;
                        TJAPlayer3.Tx.Taiko_Combo[0].vc拡大縮小倍率.X = TJAPlayer3.Skin.Game_Taiko_Combo_Scale[0];
                        TJAPlayer3.Tx.Taiko_Combo[0].t2D拡大率考慮下中心基準描画( TJAPlayer3.app.Device, rightX, TJAPlayer3.Skin.Game_Taiko_Combo_Y[nPlayer] , new Rectangle( n位の数[ i ] * TJAPlayer3.Skin.Game_Taiko_Combo_Size[0], 0, TJAPlayer3.Skin.Game_Taiko_Combo_Size[0], TJAPlayer3.Skin.Game_Taiko_Combo_Size[1]) );
				    }
                }
                else if( n桁数 <= 2 )
                {
                    //int[] arComboX = { CDTXMania.Skin.Game_Taiko_Combo_X[nPlayer] + CDTXMania.Skin.Game_Taiko_Combo_Padding[0], CDTXMania.Skin.Game_Taiko_Combo_X[nPlayer] - CDTXMania.Skin.Game_Taiko_Combo_Padding[0] };
                    if(nCombo値 < 50)
					{
						if (TJAPlayer3.Tx.Taiko_Combo[0] != null)
						{
							var yScalling = ComboScale[this.ctコンボ加算[nPlayer].n現在の値];
							TJAPlayer3.Tx.Taiko_Combo[0].vc拡大縮小倍率.Y = TJAPlayer3.Skin.Game_Taiko_Combo_Scale[0] + yScalling;
							TJAPlayer3.Tx.Taiko_Combo[0].vc拡大縮小倍率.X = TJAPlayer3.Skin.Game_Taiko_Combo_Scale[0];
							TJAPlayer3.Tx.Taiko_Combo[0].t2D拡大率考慮下中心基準描画(TJAPlayer3.app.Device, rightX - TJAPlayer3.Skin.Game_Taiko_Combo_Padding[0] * i, TJAPlayer3.Skin.Game_Taiko_Combo_Y[nPlayer], new Rectangle(n位の数[i] * TJAPlayer3.Skin.Game_Taiko_Combo_Size[0], 0, TJAPlayer3.Skin.Game_Taiko_Combo_Size[0], TJAPlayer3.Skin.Game_Taiko_Combo_Size[1]));
						}
                    }
                    else
					{
						if (TJAPlayer3.Tx.Taiko_Combo[2] != null)
						{
							var yScalling = ComboScale[this.ctコンボ加算[nPlayer].n現在の値];
							TJAPlayer3.Tx.Taiko_Combo[2].vc拡大縮小倍率.Y = TJAPlayer3.Skin.Game_Taiko_Combo_Scale[0] + yScalling;
							TJAPlayer3.Tx.Taiko_Combo[2].vc拡大縮小倍率.X = TJAPlayer3.Skin.Game_Taiko_Combo_Scale[0];
							TJAPlayer3.Tx.Taiko_Combo[2].t2D拡大率考慮下中心基準描画(TJAPlayer3.app.Device, rightX - TJAPlayer3.Skin.Game_Taiko_Combo_Padding[0] * i, TJAPlayer3.Skin.Game_Taiko_Combo_Y[nPlayer], new Rectangle(n位の数[i] * TJAPlayer3.Skin.Game_Taiko_Combo_Size[0], 0, TJAPlayer3.Skin.Game_Taiko_Combo_Size[0], TJAPlayer3.Skin.Game_Taiko_Combo_Size[1]));
						}
					}
				}
                else if( n桁数 == 3 )
                {
                    if (TJAPlayer3.Tx.Taiko_Combo[1] != null )
				    {
                        var yScalling = ComboScale_Ex[this.ctコンボ加算[nPlayer].n現在の値, 0];
                        TJAPlayer3.Tx.Taiko_Combo[1].vc拡大縮小倍率.Y = TJAPlayer3.Skin.Game_Taiko_Combo_Scale[1] + yScalling;
                        TJAPlayer3.Tx.Taiko_Combo[1].vc拡大縮小倍率.X = TJAPlayer3.Skin.Game_Taiko_Combo_Scale[1];
                        var yJumping = TJAPlayer3.Skin.Game_Taiko_Combo_Ex_IsJumping ? (int)ComboScale_Ex[this.ctコンボ加算[nPlayer].n現在の値, 1] : 0;
                        TJAPlayer3.Tx.Taiko_Combo[1].t2D拡大率考慮下中心基準描画( TJAPlayer3.app.Device, rightX - TJAPlayer3.Skin.Game_Taiko_Combo_Padding[1] * i, TJAPlayer3.Skin.Game_Taiko_Combo_Ex_Y[nPlayer] + yJumping, new Rectangle( n位の数[ i ] * TJAPlayer3.Skin.Game_Taiko_Combo_Size_Ex[0], 0, TJAPlayer3.Skin.Game_Taiko_Combo_Size_Ex[0], TJAPlayer3.Skin.Game_Taiko_Combo_Size_Ex[1]) );
                    }
                    if(TJAPlayer3.Tx.Taiko_Combo_Effect != null )
                    {
                        TJAPlayer3.Tx.Taiko_Combo_Effect.b加算合成 = true;
                        if( this.ctコンボラメ.n現在の値 < 14)
                        {
                            // ひだり
                            #region[透明度制御]
                            if (this.ctコンボラメ.n現在の値 < 7) TJAPlayer3.Tx.Taiko_Combo_Effect.Opacity = 255;
                            else if (this.ctコンボラメ.n現在の値 >= 7 && this.ctコンボラメ.n現在の値 < 14) TJAPlayer3.Tx.Taiko_Combo_Effect.Opacity = (int)(204 - (24 * this.ctコンボラメ.n現在の値));
                            #endregion
                            TJAPlayer3.Tx.Taiko_Combo_Effect.t2D拡大率考慮下中心基準描画(TJAPlayer3.app.Device, (rightX - TJAPlayer3.Skin.Game_Taiko_Combo_Padding[1] * i) - ((TJAPlayer3.Skin.Game_Taiko_Combo_Size_Ex[0] / 4) * TJAPlayer3.Skin.Game_Taiko_Combo_Scale[1]), TJAPlayer3.Skin.Game_Taiko_Combo_Ex_Y[nPlayer] - ((TJAPlayer3.Skin.Game_Taiko_Combo_Size_Ex[1] / 4) * TJAPlayer3.Skin.Game_Taiko_Combo_Scale[1]) - (int)(1.05 * this.ctコンボラメ.n現在の値));
                        }
                        if(ctコンボラメ.n現在の値 > 4 && ctコンボラメ.n現在の値 < 24)
                        {
                            // みぎ
                            #region[透明度制御]
                            if (this.ctコンボラメ.n現在の値 < 11) TJAPlayer3.Tx.Taiko_Combo_Effect.Opacity = 255;
                            else if (this.ctコンボラメ.n現在の値 >= 11 && this.ctコンボラメ.n現在の値 < 24) TJAPlayer3.Tx.Taiko_Combo_Effect.Opacity = (int)(204 - (12 * this.ctコンボラメ.n現在の値));
                            #endregion
                            TJAPlayer3.Tx.Taiko_Combo_Effect.t2D拡大率考慮下中心基準描画(TJAPlayer3.app.Device, (rightX - TJAPlayer3.Skin.Game_Taiko_Combo_Padding[1] * i) + ((TJAPlayer3.Skin.Game_Taiko_Combo_Size_Ex[0] / 4) * TJAPlayer3.Skin.Game_Taiko_Combo_Scale[1]), TJAPlayer3.Skin.Game_Taiko_Combo_Ex_Y[nPlayer] - ((TJAPlayer3.Skin.Game_Taiko_Combo_Size_Ex[1] / 4) * TJAPlayer3.Skin.Game_Taiko_Combo_Scale[1]) - (int)(1.05 * this.ctコンボラメ.n現在の値));

                        }
                        if (ctコンボラメ.n現在の値 > 14)
                        {
                            // まんなか
                            #region[透明度制御]
                            if (this.ctコンボラメ.n現在の値 < 252) TJAPlayer3.Tx.Taiko_Combo_Effect.Opacity = 255;
                            else if (this.ctコンボラメ.n現在の値 >= 22 && this.ctコンボラメ.n現在の値 < 30) TJAPlayer3.Tx.Taiko_Combo_Effect.Opacity = (int)(204 - (6 * this.ctコンボラメ.n現在の値));
                            #endregion
                            TJAPlayer3.Tx.Taiko_Combo_Effect.t2D拡大率考慮下中心基準描画(TJAPlayer3.app.Device, (rightX - TJAPlayer3.Skin.Game_Taiko_Combo_Padding[1] * i), TJAPlayer3.Skin.Game_Taiko_Combo_Ex_Y[nPlayer] - ((TJAPlayer3.Skin.Game_Taiko_Combo_Size_Ex[1] / 4) * TJAPlayer3.Skin.Game_Taiko_Combo_Scale[1]) - (int)(1.05 * this.ctコンボラメ.n現在の値));
                        }
                    }
                }
                else
                {
                    if (TJAPlayer3.Tx.Taiko_Combo[1] != null)
                    {
                        var yScalling = ComboScale_Ex[this.ctコンボ加算[nPlayer].n現在の値, 0];
                        TJAPlayer3.Tx.Taiko_Combo[1].vc拡大縮小倍率.Y = TJAPlayer3.Skin.Game_Taiko_Combo_Scale[2] + yScalling;
                        TJAPlayer3.Tx.Taiko_Combo[1].vc拡大縮小倍率.X = TJAPlayer3.Skin.Game_Taiko_Combo_Scale[2];
                        var yJumping = TJAPlayer3.Skin.Game_Taiko_Combo_Ex_IsJumping ? (int)ComboScale_Ex[this.ctコンボ加算[nPlayer].n現在の値, 1] : 0;
                        TJAPlayer3.Tx.Taiko_Combo[1].t2D拡大率考慮下中心基準描画(TJAPlayer3.app.Device, rightX - TJAPlayer3.Skin.Game_Taiko_Combo_Padding[2] * i, TJAPlayer3.Skin.Game_Taiko_Combo_Ex4_Y[nPlayer] + yJumping, new Rectangle(n位の数[i] * TJAPlayer3.Skin.Game_Taiko_Combo_Size_Ex[0], 0, TJAPlayer3.Skin.Game_Taiko_Combo_Size_Ex[0], TJAPlayer3.Skin.Game_Taiko_Combo_Size_Ex[1]));
                    }
                    if (TJAPlayer3.Tx.Taiko_Combo_Effect != null)
                    {
                        TJAPlayer3.Tx.Taiko_Combo_Effect.b加算合成 = true;
                        if (this.ctコンボラメ.n現在の値 < 14)
                        {
                            // ひだり
                            #region[透明度制御]
                            if (this.ctコンボラメ.n現在の値 < 7) TJAPlayer3.Tx.Taiko_Combo_Effect.Opacity = 255;
                            else if (this.ctコンボラメ.n現在の値 >= 7 && this.ctコンボラメ.n現在の値 < 14) TJAPlayer3.Tx.Taiko_Combo_Effect.Opacity = (int)(204 - (24 * this.ctコンボラメ.n現在の値));
                            #endregion
                            TJAPlayer3.Tx.Taiko_Combo_Effect.t2D拡大率考慮下中心基準描画(TJAPlayer3.app.Device, (rightX - TJAPlayer3.Skin.Game_Taiko_Combo_Padding[1] * i) - ((TJAPlayer3.Skin.Game_Taiko_Combo_Size_Ex[0] / 4) * TJAPlayer3.Skin.Game_Taiko_Combo_Scale[2]), TJAPlayer3.Skin.Game_Taiko_Combo_Ex4_Y[nPlayer] - ((TJAPlayer3.Skin.Game_Taiko_Combo_Size_Ex[1] / 4) * TJAPlayer3.Skin.Game_Taiko_Combo_Scale[2]) - (int)(1.05 * this.ctコンボラメ.n現在の値));
                        }
                        if (ctコンボラメ.n現在の値 > 4 && ctコンボラメ.n現在の値 < 24)
                        {
                            // みぎ
                            #region[透明度制御]
                            if (this.ctコンボラメ.n現在の値 < 11) TJAPlayer3.Tx.Taiko_Combo_Effect.Opacity = 255;
                            else if (this.ctコンボラメ.n現在の値 >= 11 && this.ctコンボラメ.n現在の値 < 24) TJAPlayer3.Tx.Taiko_Combo_Effect.Opacity = (int)(204 - (12 * this.ctコンボラメ.n現在の値));
                            #endregion
                            TJAPlayer3.Tx.Taiko_Combo_Effect.t2D拡大率考慮下中心基準描画(TJAPlayer3.app.Device, (rightX - TJAPlayer3.Skin.Game_Taiko_Combo_Padding[1] * i) + ((TJAPlayer3.Skin.Game_Taiko_Combo_Size_Ex[0] / 4) * TJAPlayer3.Skin.Game_Taiko_Combo_Scale[2]), TJAPlayer3.Skin.Game_Taiko_Combo_Ex4_Y[nPlayer] - ((TJAPlayer3.Skin.Game_Taiko_Combo_Size_Ex[1] / 4) * TJAPlayer3.Skin.Game_Taiko_Combo_Scale[2]) - (int)(1.05 * this.ctコンボラメ.n現在の値));

                        }
                        if (ctコンボラメ.n現在の値 > 14)
                        {
                            // まんなか
                            #region[透明度制御]
                            if (this.ctコンボラメ.n現在の値 < 22) TJAPlayer3.Tx.Taiko_Combo_Effect.Opacity = 255;
                            else if (this.ctコンボラメ.n現在の値 >= 22 && this.ctコンボラメ.n現在の値 < 30) TJAPlayer3.Tx.Taiko_Combo_Effect.Opacity = (int)(204 - (6 * this.ctコンボラメ.n現在の値));
                            #endregion
                            TJAPlayer3.Tx.Taiko_Combo_Effect.t2D拡大率考慮下中心基準描画(TJAPlayer3.app.Device, (rightX - TJAPlayer3.Skin.Game_Taiko_Combo_Padding[1] * i), TJAPlayer3.Skin.Game_Taiko_Combo_Ex4_Y[nPlayer] - ((TJAPlayer3.Skin.Game_Taiko_Combo_Size_Ex[1] / 4) * TJAPlayer3.Skin.Game_Taiko_Combo_Scale[2]) - (int)(1.05 * this.ctコンボラメ.n現在の値));

                        }
                    }

                }
            }

			//-----------------
			#endregion
		}

		protected virtual void tコンボ表示_ギター( int nCombo値, int nジャンプインデックス )
		{
		}
		protected virtual void tコンボ表示_ベース( int nCombo値, int nジャンプインデックス )
		{
		}
		protected void tコンボ表示_ギター( int nCombo値, int n表示中央X, int n表示中央Y, int nジャンプインデックス )
		{

		}
		protected void tコンボ表示_ベース( int nCombo値, int n表示中央X, int n表示中央Y, int nジャンプインデックス )
		{

		}
		protected void tコンボ表示_ギターベース( int nCombo値, int n表示中央X, int n表示中央Y, int nジャンプインデックス )
		{
		}


		// CActivity 実装

		public override void On活性化()
		{
			this.n現在のコンボ数 = new STCOMBO() { act = this };
			this.status = new CSTATUS();
            this.ctコンボ加算 = new CCounter[ 4 ];
			for( int i = 0; i < 4; i++ )
			{
				this.status[ i ].e現在のモード = EMode.非表示中;
				this.status[ i ].nCOMBO値 = 0;
				this.status[ i ].n最高COMBO値 = 0;
				this.status[ i ].n現在表示中のCOMBO値 = 0;
				this.status[ i ].n残像表示中のCOMBO値 = 0;
				this.status[ i ].nジャンプインデックス値 = 99999;
				this.status[ i ].n前回の時刻_ジャンプ用 = -1;
				this.status[ i ].nコンボが切れた時刻 = -1;
                this.ctコンボ加算[ i ] = new CCounter( 0, 12, 12, TJAPlayer3.Timer );
			}
            this.ctコンボラメ = new CCounter( 0, 29, 20, TJAPlayer3.Timer );
			base.On活性化();
		}
		public override void On非活性化()
		{
			if( this.status != null )
				this.status = null;

			base.On非活性化();
		}
		public override void OnManagedリソースの作成()
		{
			if( this.b活性化してない )
				return;
			base.OnManagedリソースの作成();
		}
		public override void OnManagedリソースの解放()
		{
			if( this.b活性化してない )
				return;
			base.OnManagedリソースの解放();
		}
		public override int On進行描画()
		{
			if( this.b活性化してない )
				return 0;

			for( int i = 0; i < 4; i++ )
			{
				EEvent e今回の状態遷移イベント;

				#region [ 前回と今回の COMBO 値から、e今回の状態遷移イベントを決定する。]
				//-----------------
				if( this.status[ i ].n現在表示中のCOMBO値 == this.status[ i ].nCOMBO値 )
				{
					e今回の状態遷移イベント = EEvent.同一数値;
				}
				else if( this.status[ i ].n現在表示中のCOMBO値 > this.status[ i ].nCOMBO値 )
				{
					e今回の状態遷移イベント = EEvent.ミス通知;
				}
				else if( ( this.status[ i ].n現在表示中のCOMBO値 < TJAPlayer3.ConfigIni.n表示可能な最小コンボ数.Drums ) && ( this.status[ i ].nCOMBO値 < TJAPlayer3.ConfigIni.n表示可能な最小コンボ数.Drums ) )
				{
					e今回の状態遷移イベント = EEvent.非表示;
				}
				else
				{
					e今回の状態遷移イベント = EEvent.数値更新;
				}
				//-----------------
				#endregion

				#region [ nジャンプインデックス値 の進行。]
				//-----------------
				if( this.status[ i ].nジャンプインデックス値 < 360 )
				{
					if( ( this.status[ i ].n前回の時刻_ジャンプ用 == -1 ) || ( TJAPlayer3.Timer.n現在時刻 < this.status[ i ].n前回の時刻_ジャンプ用 ) )
						this.status[ i ].n前回の時刻_ジャンプ用 = TJAPlayer3.Timer.n現在時刻;

					const long INTERVAL = 2;
					while( ( TJAPlayer3.Timer.n現在時刻 - this.status[ i ].n前回の時刻_ジャンプ用 ) >= INTERVAL )
					{
						if( this.status[ i ].nジャンプインデックス値 < 2000 )
							this.status[ i ].nジャンプインデックス値 += 3;

						this.status[ i ].n前回の時刻_ジャンプ用 += INTERVAL;
					}
				}
			//-----------------
				#endregion


			Retry:	// モードが変化した場合はここからリトライする。

				switch( this.status[ i ].e現在のモード )
				{
					case EMode.非表示中:
						#region [ *** ]
						//-----------------

						if( e今回の状態遷移イベント == EEvent.数値更新 )
						{
							// モード変更
							this.status[ i ].e現在のモード = EMode.進行表示中;
							this.status[ i ].nジャンプインデックス値 = 0;
							this.status[ i ].n前回の時刻_ジャンプ用 = TJAPlayer3.Timer.n現在時刻;
							goto Retry;
						}

						this.status[ i ].n現在表示中のCOMBO値 = this.status[ i ].nCOMBO値;
						break;
					//-----------------
						#endregion

					case EMode.進行表示中:
						#region [ *** ]
						//-----------------

						if( ( e今回の状態遷移イベント == EEvent.非表示 ) || ( e今回の状態遷移イベント == EEvent.ミス通知 ) )
						{
							// モード変更
							this.status[ i ].e現在のモード = EMode.残像表示中;
							this.status[ i ].n残像表示中のCOMBO値 = this.status[ i ].n現在表示中のCOMBO値;
							this.status[ i ].nコンボが切れた時刻 = TJAPlayer3.Timer.n現在時刻;
							goto Retry;
						}

						if( e今回の状態遷移イベント == EEvent.数値更新 )
						{
							this.status[ i ].nジャンプインデックス値 = 0;
							this.status[ i ].n前回の時刻_ジャンプ用 = TJAPlayer3.Timer.n現在時刻;
						}

						this.status[ i ].n現在表示中のCOMBO値 = this.status[ i ].nCOMBO値;
						switch( i )
						{
							case 0:
								this.tコンボ表示_太鼓( this.status[ i ].nCOMBO値, this.status[ i ].nジャンプインデックス値, 0 );
								break;

							case 1:
								this.tコンボ表示_太鼓( this.status[ i ].nCOMBO値, this.status[ i ].nジャンプインデックス値, 1 );
								break;

							case 2:
								this.tコンボ表示_ベース( this.status[ i ].nCOMBO値, this.status[ i ].nジャンプインデックス値 );
								break;

							case 3:
								this.tコンボ表示_ドラム( this.status[ i ].nCOMBO値, this.status[ i ].nジャンプインデックス値 );
								break;
						}
						break;
					//-----------------
						#endregion

					case EMode.残像表示中:
						#region [ *** ]
						//-----------------
						if( e今回の状態遷移イベント == EEvent.数値更新 )
						{
							// モード変更１
							this.status[ i ].e現在のモード = EMode.進行表示中;
							goto Retry;
						}
						if( ( TJAPlayer3.Timer.n現在時刻 - this.status[ i ].nコンボが切れた時刻 ) > 1000 )
						{
							// モード変更２
							this.status[ i ].e現在のモード = EMode.非表示中;
							goto Retry;
						}
						this.status[ i ].n現在表示中のCOMBO値 = this.status[ i ].nCOMBO値;
						break;
						//-----------------
						#endregion
				}
			}

			return 0;
		}
	}
}
