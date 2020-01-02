using System;
using System.Diagnostics;
using UnityEngine;

public class RuntimeProcessRunner
{
	public RuntimeProcessRunner(string executable, string args, string workingDirectory, int timeoutMs)
	{
		this.OnProcessSuccesful = delegate()
		{
		};
		this.OnProcessFailed = delegate()
		{
		};
		this.mExecutable = executable;
		this.mCommand = args;
		this.mWorkingDirectory = workingDirectory;
		this.mTimeoutMs = timeoutMs;
	}

	public RuntimeProcessRunner(string executable, string args)
	{
		this.OnProcessSuccesful = delegate()
		{
		};
		this.OnProcessFailed = delegate()
		{
		};
		this.mExecutable = executable;
		this.mCommand = args;
		this.mWorkingDirectory = global::RuntimeProcessRunner.DEFAULT_WORKING_DIRECTORY;
		this.mTimeoutMs = global::RuntimeProcessRunner.DEFAULT_TIMEOUT;
	}

	public global::System.Action OnProcessSuccesful { get; set; }

	public global::System.Action OnProcessFailed { get; set; }

	public bool IsComplete { get; private set; }

	public void Execute()
	{
		try
		{
			this.mProcess = new global::System.Diagnostics.Process
			{
				StartInfo = 
				{
					UseShellExecute = false,
					FileName = this.mExecutable,
					Arguments = this.mCommand,
					WorkingDirectory = this.mWorkingDirectory,
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					CreateNoWindow = true
				}
			};
			this.mProcess.OutputDataReceived += delegate(object p, global::System.Diagnostics.DataReceivedEventArgs o)
			{
				if (!string.IsNullOrEmpty(o.Data))
				{
					this.OutputLineReceived(o.Data.TrimEnd(new char[0]));
				}
			};
			this.mProcess.Start();
			this.mProcess.BeginOutputReadLine();
			string text = this.mProcess.StandardError.ReadToEnd();
			if (!this.mProcess.WaitForExit(this.mTimeoutMs))
			{
				this.ProcessFailed(true, string.Empty, -1);
			}
			else if (this.mProcess.ExitCode != 0 || text.Length != 0)
			{
				this.ProcessFailed(false, text, this.mProcess.ExitCode);
			}
			else
			{
				this.ProcessSuccesful();
			}
		}
		catch (global::System.Exception ex)
		{
			global::UnityEngine.Debug.LogError("Exception on process queue Thread: " + ex.Message + "\n" + ex.StackTrace);
		}
		finally
		{
			this.IsComplete = true;
		}
	}

	public void Abort()
	{
		this.mProcess.Kill();
	}

	protected virtual void OutputLineReceived(string line)
	{
		global::UnityEngine.Debug.Log(line);
	}

	protected virtual void ProcessSuccesful()
	{
		this.OnProcessSuccesful();
		global::UnityEngine.Debug.Log("Process Complete");
	}

	protected virtual void ProcessFailed(bool timedOut, string errorMessage, int errorCode)
	{
		this.OnProcessFailed();
		global::UnityEngine.Debug.Log(string.Format("Process Failed : {0} with code : {1}", errorMessage, errorCode));
	}

	private static readonly int DEFAULT_TIMEOUT = 40000;

	private static readonly string DEFAULT_WORKING_DIRECTORY = string.Empty;

	private string mCommand;

	private string mExecutable;

	private string mWorkingDirectory;

	private int mTimeoutMs;

	private global::System.Diagnostics.Process mProcess;
}
