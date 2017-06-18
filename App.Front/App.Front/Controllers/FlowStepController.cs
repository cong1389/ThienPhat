using App.Domain.Entities.Data;
using App.Domain.Interfaces.Services;
using App.Service.Step;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace App.Front.Controllers
{
	public class FlowStepController : FrontBaseController
	{
		private readonly IFlowStepService _flowStepService;

		public FlowStepController(IFlowStepService flowStepService)
		{
			this._flowStepService = flowStepService;
		}

		public ActionResult Index()
		{
			IEnumerable<FlowStep> flowSteps = this._flowStepService.FindBy((FlowStep x) => x.Status == 1, false);
			return base.PartialView(flowSteps);
		}
	}
}