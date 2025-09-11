using AutoMapper;
using SchoolManangement.Business.Dto;
using SchoolManangement.DataAccess.Repository.Abstract;
using SchoolManangement.DataAccess.UnitOfWorks.Abstract;
using SchoolManangement.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yonetim360Business.Mediator;

namespace SchoolManangement.Business.CQRS.Note.Commands.CreateNote
{
    public class CreateNoteCommandHandler : ICommandHandler<CreateNoteCommand, NoteDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<SchoolManangement.Entity.Note> _noteRepository;
        private readonly IRepository<ApplicationUser> _userRepository;
        public CreateNoteCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _noteRepository = _unitOfWork.GetRepository<SchoolManangement.Entity.Note>();
            _userRepository = _unitOfWork.GetRepository<ApplicationUser>();
        }

        public async Task<NoteDto> Handle(CreateNoteCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetFirstOrDefaultAsync(x => x.Id == request.UserId)??throw new Exception("User not found");

           var newNote= _mapper.Map<SchoolManangement.Entity.Note>(request);
            await _noteRepository.CreateAsync(newNote);
            await _unitOfWork.CommitAsync(cancellationToken);
            return _mapper.Map<NoteDto>(newNote);
        }
    }
}
