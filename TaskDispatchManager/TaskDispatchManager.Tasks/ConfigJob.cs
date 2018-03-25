﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Quartz;
using TaskDispatchManager.Common;

namespace TaskDispatchManager.Tasks
{
    public class ConfigJob : IJob
    {
        /// <summary>
        ///任务是否正在执行标记 ：false--未执行； true--正在执行； 默认未执行
        /// </summary>
        private static bool _isRun = false;

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                if (!_isRun)
                {
                    _isRun = true;
                    LogHelper.WriteInfoLog("Job修改任务开始,当前系统时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    //获取所有执行中的任务
                    List<TaskUtil> listTask = TaskHelper.ReadConfig().Where(e => e.IsExcute).ToList<TaskUtil>();
                    //开始对比当前配置文件和上一次配置文件之间的改变

                    //1.修改的任务
                    var updateJobList = (from p in listTask
                                         from q in TaskHelper.CurrentTaskList
                                         where p.TaskID == q.TaskID && (p.TaskParam != q.TaskParam || p.Assembly != q.Assembly || p.Class != q.Class ||
                                            p.CronExpressionString != q.CronExpressionString
                                         )
                                         select new { NewTaskUtil = p, OriginTaskUtil = q }).ToList();
                    foreach (var item in updateJobList)
                    {
                        try
                        {
                            QuartzHelper.ScheduleJob(item.NewTaskUtil);
                            //修改原有的任务
                            var index = TaskHelper.CurrentTaskList.IndexOf(item.OriginTaskUtil);
                            TaskHelper.CurrentTaskList[index] = item.NewTaskUtil;

                        }
                        catch (Exception e)
                        {
                            LogHelper.WriteErrorLog($"任务“{item.NewTaskUtil.TaskName}”配置信息更新失败！", e);
                        }
                    }

                    //2.新增的任务(TaskID在原集合不存在)
                    var addJobList = (from p in listTask
                                      where !(from q in TaskHelper.CurrentTaskList select q.TaskID).Contains(p.TaskID)
                                      select p).ToList();

                    foreach (var taskUtil in addJobList)
                    {
                        try
                        {
                            QuartzHelper.ScheduleJob(taskUtil);
                            //添加新增的任务
                            TaskHelper.CurrentTaskList.Add(taskUtil);
                        }
                        catch (Exception e)
                        {
                            LogHelper.WriteErrorLog($"任务“{taskUtil.TaskName}”新增失败！", e);
                        }
                    }

                    //3.删除的任务
                    var deleteJobList = (from p in TaskHelper.CurrentTaskList
                                         where !(from q in listTask select q.TaskID).Contains(p.TaskID)
                                         select p).ToList();
                    foreach (var taskUtil in deleteJobList)
                    {
                        try
                        {
                            QuartzHelper.DeleteJob(taskUtil.TaskID);
                            //添加新增的任务
                            TaskHelper.CurrentTaskList.Remove(taskUtil);
                        }
                        catch (Exception e)
                        {
                            LogHelper.WriteErrorLog($"任务“{taskUtil.TaskName}”删除失败！", e);
                        }
                    }
                    if (updateJobList.Count > 0 || addJobList.Count > 0 || deleteJobList.Count > 0)
                    {
                        LogHelper.WriteInfoLog("Job修改任务执行完成后,系统当前的所有任务信息:" + JsonConvert.SerializeObject(TaskHelper.CurrentTaskList));
                    }
                    else
                    {
                        LogHelper.WriteInfoLog("当前没有修改的任务");
                    }
                    _isRun = false;
                }
            }
            catch (Exception ex)
            {
                JobExecutionException e2 = new JobExecutionException(ex);
                LogHelper.WriteErrorLog("Job修改任务异常", ex);
                _isRun = false;
                //1.立即重新执行任务 
                e2.RefireImmediately = true;
            }

        }



    }
}
