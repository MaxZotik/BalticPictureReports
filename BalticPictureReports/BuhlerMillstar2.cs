using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Scada.Data.Tables;
using BalticPictureReports.Classes;

namespace BalticPictureReports
{
    public partial class BuhlerMillstar2 : Form
    {
        public BuhlerMillstar2()
        {
            InitializeComponent();
        }

        private void BuhlerMillstar2_Load(object sender, EventArgs e)
        {
            string path = @"C:\Program Files\SCADA\Reports\DailyReports";

            try
            {
                dateTimeTextBox.Text = DateTime.Now.ToString();

                DateTime dt = DateTime.Now;
                int reportRangeDays = 1;

                DateTime prevDayDT = Convert.ToDateTime(dt.AddDays(-reportRangeDays).Day.ToString() + "." + dt.AddDays(-reportRangeDays).Month.ToString() + "." 
                    + dt.AddDays(-reportRangeDays).Year.ToString() + " " + "07:30:00");

                DateTime currDayDT = Convert.ToDateTime(dt.Day.ToString() + "." + dt.Month.ToString() + "." + dt.Year.ToString()
                    + " " + "07:30:00");              

                // индексы входных каналов из списка, полученного от архива Rapid Scada
                // если в конфигурацию добавятся каналы, то эти номера могут изменится!
                int[] archScadaChNumbers = {
                    107, 110, 113,
                    207, 210, 213,
                    307, 310, 313,
                    407, 410, 413,
                    507, 510, 513,
                    607, 610, 613,
                    707, 710, 713,
                    807, 810, 813
                };

                int[] alarmLimitChNumbers = {
                    2001, 2113, 2225,
                    2008, 2120, 2232,
                    2015, 2127, 2239,
                    2022, 2134, 2246,
                    2029, 2141, 2253,
                    2036, 2148, 2260,
                    2043, 2155, 2267,
                    2050, 2162, 2274
                };

                int[] dangerLimitChNumbers = {
                    2002, 2114, 2226,
                    2009, 2121, 2233,
                    2016, 2128, 2240,
                    2023, 2135, 2247,
                    2030, 2142, 2254,
                    2037, 2149, 2261,
                    2044, 2156, 2268,
                    2051, 2163, 2275
                };

                int[] warnLimitChNumbers = {
                    2003, 2115, 2227,
                    2010, 2122, 2234,
                    2017, 2129, 2241,
                    2024, 2136, 2248,
                    2031, 2143, 2255,
                    2038, 2150, 2262,
                    2045, 2157, 2269,
                    2052, 2164, 2276
                };

                int[] normLimitChNumbers = {
                    2004, 2116, 2228,
                    2011, 2123, 2235,
                    2018, 2130, 2242,
                    2025, 2137, 2249,
                    2032, 2144, 2256,
                    2039, 2151, 2263,
                    2046, 2158, 2270,
                    2053, 2165, 2277
                };

                Control[] vibrValues = {
                    point1param3, point1param2, point1param1,
                    point2param3, point2param2, point2param1,
                    point3param3, point3param2, point3param1,
                    point8param3, point8param2, point8param1,
                    point4param3, point4param2, point4param1,
                    point5param3, point5param2, point5param1,
                    point6param3, point6param2, point6param1,
                    point7param3, point7param2, point7param1
                };

                PictureBox[] vibrTrendArrows = {
                    point1TrendArrow3, point1TrendArrow2, point1TrendArrow1,
                    point2TrendArrow3, point2TrendArrow2, point2TrendArrow1,
                    point3TrendArrow3, point3TrendArrow2, point3TrendArrow1,
                    point8TrendArrow3, point8TrendArrow2, point8TrendArrow1,
                    point4TrendArrow3, point4TrendArrow2, point4TrendArrow1,
                    point5TrendArrow3, point5TrendArrow2, point5TrendArrow1,
                    point6TrendArrow3, point6TrendArrow2, point6TrendArrow1,
                    point7TrendArrow3, point7TrendArrow2, point7TrendArrow1
                };

                PictureBox[] intergralIndicators = {
                    integralIndicator1,
                    integralIndicator2,
                    integralIndicator3,
                    integralIndicator8,
                    integralIndicator4,
                    integralIndicator5,
                    integralIndicator6,
                    integralIndicator7
                };

                RepositoryMinuteData repMinuteData = new RepositoryMinuteData(currDayDT, prevDayDT);
                RepositoryConfigLimits repConfigLimits = new RepositoryConfigLimits();

                int pointCount = archScadaChNumbers.Length;
                int repMinuteDataCount = repMinuteData.TableDatas.Rows.Count;


                for (int i = 0; i < pointCount; i++)
                {
                    int sumInd = 0, counter = 0;
                    double totalSum = 0, mSum = 0, sumIndSQR = 0, cfA = 0;

                    for (int j = 0; j < repMinuteDataCount; j++)
                    {
                        float avg = float.Parse(repMinuteData.TableDatas.Rows[j].ItemArray[1].ToString());
                        int cnlNum = int.Parse(repMinuteData.TableDatas.Rows[j].ItemArray[0].ToString());

                        if (avg > 0 && cnlNum == archScadaChNumbers[i])
                        {
                            totalSum += avg;
                            sumInd += (counter + 1);
                            sumIndSQR += Math.Pow(counter + 1, 2);
                            mSum += avg * (counter + 1);
                            counter += 1;
                        }
                    }

                    if (counter > 0)
                    {
                        char[] separators = new char[] { '.', ',' };
                        double average = totalSum / counter;

                        if (average % 1 != 0)
                        {
                            string[] splitRes = average.ToString().Split(separators, StringSplitOptions.RemoveEmptyEntries);
                            vibrValues[i].Text = splitRes[0] + "," + splitRes[1].Substring(0, 2);
                        }
                        else
                        {
                            vibrValues[i].Text = average + ",00";
                        }


                        float alarmLimit = float.Parse(repConfigLimits.TableDatas.Select($"CnlNum = {alarmLimitChNumbers[i]}").First().ItemArray[1].ToString());

                        if (average > alarmLimit)
                        {
                            vibrValues[i].BackColor = Color.FromArgb(192, 0, 0);
                        }
                        else
                        {
                            float dangerLimit = float.Parse(repConfigLimits.TableDatas.Select($"CnlNum = {dangerLimitChNumbers[i]}").First().ItemArray[1].ToString());

                            if (average > dangerLimit)
                            {
                                vibrValues[i].BackColor = Color.FromArgb(236, 118, 0);
                            }
                            else
                            {
                                float warnLimit = float.Parse(repConfigLimits.TableDatas.Select($"CnlNum = {warnLimitChNumbers[i]}").First().ItemArray[1].ToString());

                                if (average > warnLimit)
                                {
                                    vibrValues[i].BackColor = Color.FromArgb(232, 185, 0);
                                }
                                else
                                {
                                    float normLimit = float.Parse(repConfigLimits.TableDatas.Select($"CnlNum = {normLimitChNumbers[i]}").First().ItemArray[1].ToString());

                                    if (average > normLimit)
                                    {
                                        vibrValues[i].BackColor = Color.FromArgb(75, 102, 25);
                                    }
                                    else
                                    {
                                        vibrValues[i].BackColor = Color.FromArgb(65, 105, 225);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        vibrValues[i].Text = "0,00";
                        vibrValues[i].BackColor = Color.Gray;
                        vibrTrendArrows[i].Image = Properties.Resources.Серый_квадрат_заплатка;
                    }
                    vibrValues[i].ForeColor = Color.White;

                    if (i % 3 == 2)
                    {
                        if (Convert.ToInt32(vibrValues[i].BackColor.R) == 192 || Convert.ToInt32(vibrValues[i - 1].BackColor.R) == 192 || Convert.ToInt32(vibrValues[i - 2].BackColor.R) == 192)
                        {
                            intergralIndicators[i / 3].Image = Properties.Resources.Red_Circle_34x34;
                        }
                        else
                        {
                            if (Convert.ToInt32(vibrValues[i].BackColor.R) == 236 || Convert.ToInt32(vibrValues[i - 1].BackColor.R) == 236 || Convert.ToInt32(vibrValues[i - 2].BackColor.R) == 236)
                            {
                                intergralIndicators[i / 3].Image = Properties.Resources.Orange_Circle_34x34;
                            }
                            else
                            {
                                if (Convert.ToInt32(vibrValues[i].BackColor.R) == 232 || Convert.ToInt32(vibrValues[i - 1].BackColor.R) == 232 || Convert.ToInt32(vibrValues[i - 2].BackColor.R) == 232)
                                {
                                    intergralIndicators[i / 3].Image = Properties.Resources.Yellow_Circle_34x34;
                                }
                                else
                                {
                                    if (Convert.ToInt32(vibrValues[i].BackColor.R) == 75 || Convert.ToInt32(vibrValues[i - 1].BackColor.R) == 75 || Convert.ToInt32(vibrValues[i - 2].BackColor.R) == 75)
                                    {
                                        intergralIndicators[i / 3].Image = Properties.Resources.Green_Circle_34x34;
                                    }
                                    else
                                    {
                                        if (Convert.ToInt32(vibrValues[i].BackColor.R) == 65 || Convert.ToInt32(vibrValues[i - 1].BackColor.R) == 65 || Convert.ToInt32(vibrValues[i - 2].BackColor.R) == 65)
                                        {
                                            intergralIndicators[i / 3].Image = Properties.Resources.Green_blue_NEW_34x34;
                                        }
                                        else
                                        {
                                            intergralIndicators[i / 3].Image = Properties.Resources.Green_gray_34x34;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    cfA = (counter * mSum - sumInd * totalSum) / (counter * sumIndSQR - sumInd * sumInd);

                    if (Convert.ToInt32(vibrValues[i].BackColor.R) == 192)
                    {
                        if (cfA > 0)
                        {
                            vibrTrendArrows[i].Image = Properties.Resources.Стрелка_вверх_красная;
                        }
                        else
                        {
                            if (cfA < 0)
                            {
                                vibrTrendArrows[i].Image = Properties.Resources.Стрелка_вниз_красная;
                            }
                            else
                            {
                                vibrTrendArrows[i].Image = Properties.Resources.Красный_квадрат_заплатка;
                            }
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(vibrValues[i].BackColor.R) == 236)
                        {
                            if (cfA > 0)
                            {
                                vibrTrendArrows[i].Image = Properties.Resources.Стрелка_вверх_оранж;
                            }
                            else
                            {
                                if (cfA < 0)
                                {
                                    vibrTrendArrows[i].Image = Properties.Resources.Стрелка_вниз_оранж;
                                }
                                else
                                {
                                    vibrTrendArrows[i].Image = Properties.Resources.Оранжевый_квадрат_заплатка;
                                }
                            }
                        }
                        else
                        {
                            if (Convert.ToInt32(vibrValues[i].BackColor.R) == 232)
                            {
                                if (cfA > 0)
                                {
                                    vibrTrendArrows[i].Image = Properties.Resources.Стрелка_вверх_жёлтая;
                                }
                                else
                                {
                                    if (cfA < 0)
                                    {
                                        vibrTrendArrows[i].Image = Properties.Resources.Стрелка_вниз_жёлтая;
                                    }
                                    else
                                    {
                                        vibrTrendArrows[i].Image = Properties.Resources.Жёлтый_квадрат_заплатка;
                                    }
                                }
                            }
                            else
                            {
                                if (Convert.ToInt32(vibrValues[i].BackColor.R) == 75)
                                {
                                    if (cfA > 0)
                                    {
                                        vibrTrendArrows[i].Image = Properties.Resources.Стрелка_вверх_зелёная;
                                    }
                                    else
                                    {
                                        if (cfA < 0)
                                        {

                                            vibrTrendArrows[i].Image = Properties.Resources.Стрелка_вниз_зелёная;
                                        }
                                        else
                                        {
                                            vibrTrendArrows[i].Image = Properties.Resources.Зелёный_квадрат_заплатка;
                                        }
                                    }
                                }
                                else
                                {
                                    if (Convert.ToInt32(vibrValues[i].BackColor.R) == 65)
                                    {
                                        if (cfA > 0)
                                        {
                                            vibrTrendArrows[i].Image = Properties.Resources.Стрелка_вверх_синяя;
                                        }
                                        else
                                        {
                                            if (cfA < 0)
                                            {
                                                vibrTrendArrows[i].Image = Properties.Resources.Стрелка_вниз_синяя;
                                            }
                                            else
                                            {
                                                vibrTrendArrows[i].Image = Properties.Resources.Синий_квадрат_заплатка;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                pictureBox1.BringToFront();
                for (int i = 0; i < vibrTrendArrows.Length; i++)
                {
                    vibrTrendArrows[i].SendToBack();
                }
                var ctrl = this;
                Bitmap bmp = new Bitmap(ctrl.Width, ctrl.Height);
                Point point = new Point(0, 0);
                ctrl.DrawToBitmap(bmp, new Rectangle(point, bmp.Size));
                string savePath = path + @"\Reports\" +
                      DateTime.Now.Year.ToString() +
                      (DateTime.Now.Month >= 10 ? DateTime.Now.Month.ToString() : "0" + DateTime.Now.Month.ToString()) +
                      (DateTime.Now.Day >= 10 ? DateTime.Now.Day.ToString() : "0" + DateTime.Now.Day.ToString()) +
                      " BM2DailyReport.png";
                //bmp.Save(savePath);

                DirectoryFile.CheckDirectory(path + @"\Reports\");

                using (var bitmap = new Bitmap(bmp))
                { 
                    bitmap.Save(savePath); 
                }

                this.Close();

                Log.WriteLog("Формирование суточного отчёта 'Дробилка мокрого помола Buhler Millstar 2' успешно завершено!");
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.ToString());
            }
        }
    }
}
